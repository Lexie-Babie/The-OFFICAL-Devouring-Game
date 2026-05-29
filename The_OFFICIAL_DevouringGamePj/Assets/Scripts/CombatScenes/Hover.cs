using UnityEngine;

public class Hover : MonoBehaviour
{
    public float amplitude = 0.5f; // How high/low it goes
    public float speed = 1.0f;     // How fast it moves
    private float startY;

    void Start()
    {
        startY = transform.position.y;
    }

    void Update()
    {
        float newY = startY + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

}
