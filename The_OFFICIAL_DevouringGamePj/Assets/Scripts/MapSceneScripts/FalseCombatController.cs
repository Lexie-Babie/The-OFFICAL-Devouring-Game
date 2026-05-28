using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FalseCombatController : MonoBehaviour
{
    public void OnStartClick()
    {
        Debug.Log("return to MapScene button clicked");
        SceneManager.LoadScene("MapScene");
    }
}
