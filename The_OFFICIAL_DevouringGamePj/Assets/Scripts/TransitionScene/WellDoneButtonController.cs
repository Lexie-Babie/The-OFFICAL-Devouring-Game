using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class WellDoneButtonController : MonoBehaviour
{
    public void OnNextClick()
    {
        SceneManager.LoadScene("ThanksForPlaying");
    }
}

