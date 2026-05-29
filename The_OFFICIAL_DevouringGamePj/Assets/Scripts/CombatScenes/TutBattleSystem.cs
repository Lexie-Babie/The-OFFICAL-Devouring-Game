using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST } //defining the different states that the game can be inusing UnityEngine;

public class TutBattleSystem : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject goblinPrefab;

    public Transform playerBattleStation;
    public Transform goblinBattleStation;

    Unit playerUnit;
    Unit goblinUnit;

    public DialogueManager dialogueManager;
    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;
    public BattleHUD goblinHUD;

    public BattleHUD playerHealth;
    public BattleHUD goblinHealth;

    public bool SetActive;
    public bool isPlayerTurn = true;
    public bool isEnemyTurn = false;

    public Button AttackButton;
    public Button CookButton;
    public Button HealButton;
    public float cooldownTime = 1.0f;

    public Transform respawnPoint;
    public AudioSource backgroundMusic;

    public Image flashImage;
    public float flashDuration = 0.2f;
    public Color flashColor = new Color(1, 0, 0, 0.5f); // Red with 50% transparency

    [Header("Stats")]
    public int normalDamage = 10;
    public int burstDamage = 30;
    public float actionDelay = 1f;

    private int turnCounter = 0;


    public BattleState state;

    //Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(goblinPrefab, goblinBattleStation);
        goblinUnit = enemyGO.GetComponent<Unit>();

        dialogueManager.dialogueText.text = "A strange " + goblinUnit.unitName + " has appeared...";

        playerHUD.SetHUD(playerUnit);
        goblinHUD.SetHUD(goblinUnit);

        playerHealth.SetSlider(playerUnit);
        goblinHealth.SetSlider(goblinUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();
            if (enemy != null)
            {
                enemy.TakeDamage(10f);
            }
        }
    }

    IEnumerator PlayerAttack()
    {
        isEnemyTurn = true;
        isPlayerTurn = false;

        StartCoroutine(ButtonCooldownRoutine());

        bool isDead = goblinUnit.TakeDamage(playerUnit.damage);
        Debug.Log("Takedamage");

        goblinHealth.SetHP(goblinUnit.currentHP);
        dialogueManager.dialogueText.text = "The attack is successful! " + goblinUnit.unitName + " takes " + playerUnit.damage + " damage!";

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.WON;

            goblinHealth.SetHP(goblinUnit.currentHP = 0);
            goblinUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(1.5f);

            SceneManager.LoadScene("WinScreen");
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            goblinHealth.SetHP(goblinUnit.currentHP);

            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EnemyTurn2());
        }
       
    }

    IEnumerator PlayerCook(ItemData item)
    {
        isEnemyTurn = true;
        isPlayerTurn = false;

        StartCoroutine(ButtonCooldownRoutine());

        // Use the item's value as damage instead of playerUnit.cook
        int damage = item.value;
        bool isDead = goblinUnit.CookDamage(damage);

        // Remove the item from the inventory after use
        InventoryManager.Instance.RemoveItem(item);

        goblinHealth.SetHP(goblinUnit.currentHP);
        dialogueManager.dialogueText.text = playerUnit.unitName + " used " + item.itemName + "! " + goblinUnit.unitName + " takes " + damage + " damage!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            goblinHealth.SetHP(goblinUnit.currentHP = 0);
            goblinUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3.0f);
            SceneManager.LoadScene("WinScreen");
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            goblinHealth.SetHP(goblinUnit.currentHP);
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(EnemyTurn2());
        }
    }


    public void StartButtonCooldown()
    {
        StartCoroutine(ButtonCooldownRoutine());
    }

    private IEnumerator ButtonCooldownRoutine()
    {
        if (!isPlayerTurn)
        {
            yield return new WaitForSeconds(1.5f); // Wait for 2 seconds before disabling the button
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

        yield return new WaitForSeconds(1.5f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    public void StartEnemyTurn()
    {
        turnCounter++;
        StartCoroutine(EnemyTurn2());
    }
    private IEnumerator EnemyTurn2()
    {
        state = BattleState.ENEMYTURN;
        // Pause briefly before the AI decides/acts
        yield return new WaitForSeconds(actionDelay);

        // Check if the current turn is a multiple of 3
        if (turnCounter % 3 == 0)
        {
            yield return StartCoroutine(BurstAttack());
            dialogueManager.dialogueText.text = playerUnit.unitName + " takes " + goblinUnit.maxDamage + " damage!";
        }
        else
        {
            yield return StartCoroutine(NormalAttack());
            dialogueManager.dialogueText.text = playerUnit.unitName + " takes " + goblinUnit.minDamage + " damage!";
        }
        TriggerFlash(); 

        bool isDead = playerUnit.TakeDamage(goblinUnit.maxDamage);
        bool isDeadNormal = playerUnit.TakeDamage(goblinUnit.minDamage);
        playerHealth.SetHP(playerUnit.currentHP);
    

        yield return new WaitForSeconds(1.5f);

        if (isDead)
        {
            state = BattleState.LOST;
            playerUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(1.5f);

            EndBattle();
        }
        else if (isDeadNormal)
        {
            state = BattleState.LOST;
            playerUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(1.5f);
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;

            PlayerTurn();
        }
    }

    private IEnumerator NormalAttack()
    {
        dialogueManager.dialogueText.text = goblinUnit.unitName + " attacks with a NORMAL ATTACK!";
        int minDamage = goblinUnit.minDamage;
        Debug.Log("Enemy uses a Normal Attack!");
        // Add your attack animation and damage application logic here
        yield return new WaitForSeconds(1.5f); // Wait for animation
    }

    private IEnumerator BurstAttack()
    {
        dialogueManager.dialogueText.text = goblinUnit.unitName + " is preparing a BURST ATTACK!";
        int maxDamage = goblinUnit.maxDamage;
        Debug.Log("Enemy triggers a BURST ATTACK!");
        // Add your heavy attack animation and damage application logic here
        yield return new WaitForSeconds(1.5f); // Wait for burst animation
    }


    IEnumerator EnemyTurn()
    {
        dialogueManager.dialogueText.text = goblinUnit.unitName + " attacks!";

        yield return new WaitForSeconds(1.5f); 
        
        TriggerFlash();

        bool isDead = playerUnit.TakeDamage(goblinUnit.damage);
        playerHealth.SetHP(playerUnit.currentHP);
        dialogueManager.dialogueText.text = playerUnit.unitName + " takes " + playerUnit.damage + " damage!";

        yield return new WaitForSeconds(1.5f);

        if (isDead)
        {
            state = BattleState.LOST;
            playerUnit.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(3.0f);

            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            
            PlayerTurn();
            
        }
    }


    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            state = BattleState.ENEMYTURN;
            // Randomly offset position within magnitude range
            transform.localPosition = (Vector3)Random.insideUnitCircle * magnitude + originalPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueManager.dialogueText.text = "You have defeated the " + goblinUnit.unitName + "!";

            if (backgroundMusic.isPlaying)
            {
                backgroundMusic.Stop();
            }
        }
        else if (state == BattleState.LOST)
        {
            dialogueManager.dialogueText.text = "You were defeated...";

            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    /*void Update()
    {
        if (state == BattleState.WON)
        {
            SceneManager.LoadScene("WinScreen");
        }
        else if (state == BattleState.LOST)
        {
            SceneManager.LoadScene("LoseScreen");
        }
    }*/

    void PlayerTurn()
    {
        isEnemyTurn = false;
        isPlayerTurn = true;
        StartCoroutine(ButtonCooldownRoutine());
        dialogueManager.dialogueText.text = "Choose an action, " + playerUnit.unitName;
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;
        StartCoroutine(PlayerAttack());

    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN) return;

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
        if (state != BattleState.PLAYERTURN) return;

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
