using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState3 { START, PLAYERTURN, ENEMYTURN, WON, LOST } //defining the different states that the game can be inusing UnityEngine;
public class EndBattleScene : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject finalbossPrefab;

    public Transform playerBattleStation;
    public Transform finalbossBattleStation;

    Unit playerUnit;
    Unit finalbossUnit;

    public DialogueManager dialogueManager;
    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD finalbossHUD;

    public Slider hpSlider;

    public BattleHUD playerHealth;
    public BattleHUD finalbossHealth;

    public bool SetActive;
    public bool isPlayerTurn = true;
    public bool isEnemyTurn = false;

    public Button AttackButton;
    public Button CookButton;
    public Button HealButton;
    public float cooldownTime = 1.0f;

    public Transform respawnPoint;

    public AudioSource backgroundMusic;

    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hitSound;

    public bool isShaking = false;
    public float shakeDuration = 2.5f;
    public float shakeMagnitude = 1.1f;
    public float dampingSpeed = 1.0f;
    public Camera maincamera;

    public int enemymaxHealth = 100;
    public int enemycurrentHealth;
    public int randomHeal = 10;
    public float randomHealChance = 0.2f; // 20% chance to heal

    public BurstAttackController burstAttackController;
    public int attackA = 10;
    public int attackB = 15;
    public int attackC = 20;
    public int attackD = 0;

    public bool randomBurstAttack = true;

    public Image flashImage;
    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1, 0, 0, 0.5f); // Red with 50% transparency

    public BattleState3 state;

    //Start is called before the first frame update
    void Start()
    {
        state = BattleState3.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(finalbossPrefab, finalbossBattleStation);
        finalbossUnit = enemyGO.GetComponent<Unit>();

        dialogueManager.dialogueText.text = "A viscous " + finalbossUnit.unitName + " has appeared...";

        playerHUD.SetHUD(playerUnit);
        finalbossHUD.SetHUD(finalbossUnit);

        playerHealth.SetSlider(playerUnit);
        finalbossHealth.SetSlider(finalbossUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState3.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        isEnemyTurn = true;
        isPlayerTurn = false;

        StartCoroutine(ButtonCooldownRoutine());

        bool isDead = finalbossUnit.TakeDamage(playerUnit.damage);
        Debug.Log("Takedamage");

        finalbossHealth.SetHP(finalbossUnit.currentHP);
        dialogueManager.dialogueText.text = "The attack is successful! " + finalbossUnit.unitName + " takes " + playerUnit.damage + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState3.WON;

            finalbossHealth.SetHP(finalbossUnit.currentHP = 0);
            finalbossUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(2f);

            SceneManager.LoadScene("WinScreen");
            EndBattle();
        }
        else
        {
            state = BattleState3.ENEMYTURN;
            finalbossHealth.SetHP(finalbossUnit.currentHP);
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerCook(ItemData item)
    {
        isEnemyTurn = true;
        isPlayerTurn = false;

        StartCoroutine(ButtonCooldownRoutine());

        // Use the item's value as damage instead of playerUnit.cook
        int damage = item.value;
        bool isDead = finalbossUnit.CookDamage(damage);

        // Remove the item from the inventory after use
        InventoryManager.Instance.RemoveItem(item);

        finalbossHealth.SetHP(finalbossUnit.currentHP);
        dialogueManager.dialogueText.text = playerUnit.unitName + " used " + item.itemName + "! " + finalbossUnit.unitName + " takes " + damage + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState3.WON;
            finalbossHealth.SetHP(finalbossUnit.currentHP = 0);
            finalbossUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("WinScreen");
            EndBattle();
        }
        else
        {
            state = BattleState3.ENEMYTURN;
            finalbossHealth.SetHP(finalbossUnit.currentHP);
            yield return new WaitForSeconds(2f);
            StartCoroutine(EnemyTurn());
        }
    }


    IEnumerator PlayerHeal(ItemData item)
    {
        isEnemyTurn = true;
        isPlayerTurn = false;

        StartCoroutine(ButtonCooldownRoutine());

        // Use the item's value as the heal amount
        int healAmount = item.value;
        playerUnit.Heal(healAmount);

        // Remove the item from the inventory after use
        InventoryManager.Instance.RemoveItem(item);

        playerHealth.SetHP(playerUnit.currentHP);
        dialogueManager.dialogueText.text = playerUnit.unitName + " used " + item.itemName + "! +" + healAmount + " HP restored!";

        yield return new WaitForSeconds(2f);

        state = BattleState3.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }


    public void StartButtonCooldown()
    {
        StartCoroutine(ButtonCooldownRoutine());
    }

    private IEnumerator ButtonCooldownRoutine()
    {
        if (!isPlayerTurn)
        {
            yield return new WaitForSeconds(1.5f); // Wait for 1.5 seconds before disabling the button
            AttackButton.interactable = false; // Disable button
            AttackButton.gameObject.SetActive(false); // Hide button
            CookButton.interactable = false;
            CookButton.gameObject.SetActive(false);
            HealButton.interactable = false;
            HealButton.gameObject.SetActive(false);
        }


        yield return new WaitForSeconds(cooldownTime);

        if (isPlayerTurn)
        {
            AttackButton.interactable = true; // Re-enable button
            AttackButton.gameObject.SetActive(true); // Show button
            CookButton.interactable = true;
            CookButton.gameObject.SetActive(true);
            HealButton.interactable = true;
            HealButton.gameObject.SetActive(true);
        }
    }

    public void TriggerFlash()
    {
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        // Set to flash color
        flashImage.color = flashColor;

        // Fade out
        float elapsed = 0f;
        while (elapsed < flashDuration)
        {
            elapsed += Time.deltaTime;
            float lerpAlpha = Mathf.Lerp(flashColor.a, 0, elapsed / flashDuration);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, lerpAlpha);
            yield return null;
        }

        // Ensure it's fully transparent at the end
        flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, 0);
    }

    public void TriggerRandomBurst()
    {
        StartCoroutine(burstAttackController.PerformBurstRoutine());
        finalbossUnit.GetComponent<BurstAttackController>().TriggerRandomBurst();
    }

    IEnumerator EnemyTurn()
    {
        isShaking = true;
        dialogueManager.dialogueText.text = finalbossUnit.unitName + " attacks!";

        yield return new WaitForSeconds(2f);
        TriggerFlash();
        bool isDead = playerUnit.TakeDamage(finalbossUnit.damage);


        playerHealth.SetHP(playerUnit.currentHP);
        TriggerFlash();
        dialogueManager.dialogueText.text = playerUnit.unitName + " takes " + playerUnit.damage + " damage!";

        // bossUnit.GetComponent<BurstAttackController>().TriggerRandomBurst();


        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState3.LOST;
            playerUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3.0f);

            ; SceneManager.LoadScene("LoseScreen");
            EndBattle();
        }
        else
        {
            state = BattleState3.PLAYERTURN;
            isShaking = false;

            PlayerTurn();
        }
    }

    public void StartCameraShake()
    {
        StartCoroutine(Shake(shakeDuration, shakeMagnitude));
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        isShaking = true;
        isEnemyTurn = true;
        state = BattleState3.ENEMYTURN;
        Vector3 originalPos = transform.localPosition;
        float elapsed = 3.0f;
        while (elapsed < duration)
        {
            // Randomly offset position within magnitude range
            transform.localPosition = (Vector3)Random.insideUnitCircle * magnitude + originalPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        isShaking = false;
        transform.localPosition = originalPos;
    }

    void EndBattle()
    {
        if (state == BattleState3.WON)
        {
            dialogueManager.dialogueText.text = "You have defeated the " + finalbossUnit.unitName + "!";

            SceneManager.LoadScene("WinScreen");

            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
        }
        else if (state == BattleState3.LOST)
        {
            dialogueManager.dialogueText.text = "You were defeated...";

            SceneManager.LoadScene("LoseScreen");

            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void PlayerTurn()
    {
        isEnemyTurn = false;
        isPlayerTurn = true;
        StartCoroutine(ButtonCooldownRoutine());
        dialogueManager.dialogueText.text = "Choose an action, " + playerUnit.unitName;
    }

    public void OnAttackButton()
    {
        if (state != BattleState3.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState3.PLAYERTURN) return;

        // Check if heal inventory is empty
        if (InventoryManager.Instance.healInventory.Count == 0)
        {
            dialogueManager.dialogueText.text = "No healing items left!";
            return;
        }

        // Open item selection panel with heal items
        CombatInventoryUI.Instance.ShowHealItems(OnHealItemSelected);
    }
    void OnHealItemSelected(ItemData item)
    {
        StartCoroutine(PlayerHeal(item));
    }

    public void OnCookButton()
    {
        if (state != BattleState3.PLAYERTURN) return;

        // Check if damage inventory is empty
        if (InventoryManager.Instance.damageInventory.Count == 0)
        {
            dialogueManager.dialogueText.text = "No ingredients to cook with!";
            return;
        }

        // Open item selection panel with damage items
        CombatInventoryUI.Instance.ShowDamageItems(OnDamageItemSelected);
    }

    void OnDamageItemSelected(ItemData item)
    {
        StartCoroutine(PlayerCook(item));
    }
}

