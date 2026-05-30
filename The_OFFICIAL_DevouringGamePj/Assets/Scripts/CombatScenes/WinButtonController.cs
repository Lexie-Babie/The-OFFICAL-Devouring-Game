using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WinButtonController : MonoBehaviour
{
    public void OnNextClickk()
    {
        SceneManager.LoadScene("MapScene");
    }
}
