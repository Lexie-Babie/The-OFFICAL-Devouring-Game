using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{

    public TextMeshProUGUI dialogueText;
    public GameObject dialogueBox;
    public float typeSpeed = 0.05f;

    public IEnumerator ShowDialogue(string line)
    {
        dialogueBox.SetActive(true);
        dialogueText.text = "";

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }

    }
}
