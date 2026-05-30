using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LoseButtonController : MonoBehaviour
{
    public void OnNextClickk()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("saved data all cleared");
        SceneManager.LoadScene("TitleScene");
    }
}
