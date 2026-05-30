using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class TitleSceneController : MonoBehaviour
{
  public void OnStartClick()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void OnExitClick()
    {
        Application.Quit();
    }
}
