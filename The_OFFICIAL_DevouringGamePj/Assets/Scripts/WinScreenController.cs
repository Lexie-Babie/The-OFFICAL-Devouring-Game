using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenController : MonoBehaviour
{
    public void OnNextClick()
    {
        SceneManager.LoadScene("MapScene");
    }
}
