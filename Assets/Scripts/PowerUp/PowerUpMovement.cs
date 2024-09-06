using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    public float rotationSpeed = 50f;
    public float bounceSpeed = 2f;
    public float bounceHeight = 0.5f;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = initialRotation * Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
        float newY = Mathf.Sin(Time.time * bounceSpeed) * bounceHeight + initialPosition.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}