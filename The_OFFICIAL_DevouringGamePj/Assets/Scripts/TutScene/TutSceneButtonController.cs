using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TutSceneButtonController : MonoBehaviour
{
   public void OnContinueClick()
    {
        SceneManager.LoadScene("MapScene");
    }
}
