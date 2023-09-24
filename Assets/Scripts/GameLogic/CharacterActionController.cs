using UnityEngine;

public class CharacterActionController : MonoBehaviour
{
    public float speed = 5f; // Speed of character

    private Vector3 targetPosition; // Target position
    private bool isMoving = false; // Check state of character

    Vector2 m_CurrentLookDirection;
    
    [SerializeField]
    Animator m_Animator;

    private int m_DirXHash = Animator.StringToHash("DirX");
    private int m_DirYHash = Animator.StringToHash("DirY");
    private int m_SpeedHash = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Start()
    {
        m_CurrentLookDirection = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Left mouse
        if (Input.GetMouseButton(0))
        {
            // If user is clicking button UI -> return
            if (EventButtonManager.GetIsClickingButton()) return;

            // Use ray to check if user click on Tilemap
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null && hit.collider.gameObject.tag == "Tilemap")
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;

                targetPosition = mousePosition;
                isMoving = true;
            }
        }

        // If character moving
        if (isMoving)
        {
            // Distance user must go to the target
            Vector3 direction = targetPosition - transform.position;

            // Set Animator for user
            SetLookDirectionFrom(direction);

            // Distance user can move in every deltaTime
            float distance = speed * Time.deltaTime;

            // If distance user must go bigger (>) distance user can go in delta time -> user must keep going
            if (direction.magnitude > distance)
            {
                transform.Translate(direction.normalized * distance);
            }
            else
            {
                // Else user go to the target choosed and end moving
                transform.position = targetPosition;
                isMoving = false;
            }
        }

        m_Animator.SetFloat(m_DirXHash, m_CurrentLookDirection.x);
        m_Animator.SetFloat(m_DirYHash, m_CurrentLookDirection.y);
        if (isMoving)
        {
            m_Animator.SetFloat(m_SpeedHash, speed);
        }
        else
        {
            m_Animator.SetFloat(m_SpeedHash, 0);
        }
    }


    void SetLookDirectionFrom(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            m_CurrentLookDirection = direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            m_CurrentLookDirection = direction.y > 0 ? Vector2.up : Vector2.down;
        }
    }
}
