using UnityEngine;

public class PaddleController : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private Rigidbody2D rb;

    private float direction = 0f;

    private void FixedUpdate()
    {
        if (direction != 0f)
        {
            rb.linearVelocityY = direction * speed;
        }
    }
    public void SetDirection(float newDirection)
    {
        direction = newDirection;
    }
}
