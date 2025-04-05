using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Rigidbody2D rb;

    private float direction = 0f;
    // Update is called once per frame
    private void Update()
    {
        HandleMovement();

    }

    private void FixedUpdate()
    {
        if(direction != 0f)
        {
            rb.linearVelocityY = direction * speed;
        }
    }
    private void HandleMovement()
    {
        direction = 0f;
        if (Input.GetKey(KeyCode.W))
        {
            direction = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction = -1f;
        }
    }
}
