using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // Transform of character
    public float smoothSpeed = 5f; // Speed of camera for smooth

    public Vector3 offset = new Vector3 (0, 0, -10); // Positon of character in main screen

    // LateUpdate run after function Update run
    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            transform.position = smoothedPosition;
        }
    }
}
