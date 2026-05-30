using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ThankyouButtonController : MonoBehaviour
{
    public void OnBackClick()
    {
        SceneManager.LoadScene("TitleScene");
    }
}
