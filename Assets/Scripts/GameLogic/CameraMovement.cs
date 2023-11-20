using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target; // Transform of character
    public float smoothSpeed = 5f; // Speed of camera for smooth

    public Vector3 offset = new Vector3(0, 0, -10); // Positon of character in main screen

    public Vector3 minValues, maxValue;

    public float zoomSpeed = 0.3f;
    public float minZoom = 3f;
    public float maxZoom = 10f;

    private void Update()
    {
        if (Input.touchCount >= 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float zoomFactor = deltaMagnitudeDiff * zoomSpeed;

            float newSize = Camera.main.orthographicSize + zoomFactor;
            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

            float zoomRatio = newSize / Camera.main.orthographicSize;

            float adjustedZoomSpeed = zoomSpeed * Camera.main.orthographicSize;

            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, newSize, Time.deltaTime * adjustedZoomSpeed);

            Camera.main.transform.localScale /= zoomRatio;
        }
    }

    // LateUpdate run after function Update run
    /*    private void LateUpdate()
        {
            if (target != null)
            {
                Vector3 desiredPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                transform.position = smoothedPosition;
            }
        }*/
    private void FixedUpdate()
    {
        Follow();
    }
    void Follow()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            //Verify if the targetPosition is out of bound or not
            //Limit it to the min and max values
            Vector3 boundPosition = new Vector3(
                Mathf.Clamp(targetPosition.x, minValues.x, maxValue.x),
                Mathf.Clamp(targetPosition.y, minValues.y, maxValue.y),
                Mathf.Clamp(targetPosition.z, minValues.z, maxValue.z));

            Vector3 smoothPosition = Vector3.Lerp(transform.position, boundPosition, smoothSpeed * Time.fixedDeltaTime);
            transform.position = smoothPosition;
        }
    }
}