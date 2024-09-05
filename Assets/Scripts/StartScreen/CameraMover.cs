using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public float speed = 10f;
    
    void Update()
    {
        // Simple circular movement around the origin point
        transform.RotateAround(Vector3.zero, Vector3.up, speed * Time.deltaTime);
    }
}