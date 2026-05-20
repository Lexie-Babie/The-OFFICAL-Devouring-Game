using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CameraShake cameraShake;

    void Update()
    {
        // Example: Press 'Space' to shake the camera
        if (Input.GetButtonDown(KeyCode.OnAttackButtonClick))
        {
            cameraShake.TriggerShake(0.5f, 0.2f);
        }
    }
}
