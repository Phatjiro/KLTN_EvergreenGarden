using System.Collections;
using System.Collections.Generic;
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
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            targetPosition = mousePosition;
            isMoving = true;
        }

        // If character moving
        if (isMoving)
        { 
            Vector3 direction = targetPosition - transform.position;
            SetLookDirectionFrom(direction);
            float distance = speed * Time.deltaTime;

            if (direction.magnitude > distance)
            {
                transform.Translate(direction.normalized * distance);
            }
            else
            { 
                transform.position = targetPosition;
                isMoving = false;
            }
        }

        m_Animator.SetFloat(m_DirXHash, m_CurrentLookDirection.x);
        m_Animator.SetFloat(m_DirYHash, m_CurrentLookDirection.y);
        if (isMoving)
            m_Animator.SetFloat(m_SpeedHash, speed);
        else
            m_Animator.SetFloat(m_SpeedHash, 0);
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
