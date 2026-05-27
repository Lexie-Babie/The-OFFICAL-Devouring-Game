using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue; 


    public void TriggerDialogue ()
    {
        Object.FindFirstObjectByType<DialogueManager2>().StartDialogue(dialogue);
    }
}
