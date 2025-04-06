using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PaddleController))]
public class PlayerController : MonoBehaviour
{
    private PaddleController paddle;
    // Update is called once per frame
    private void Awake()
    {
        paddle = GetComponent<PaddleController>();
    }
    private void Update()
    {
        HandleMovement();

    }
    private void HandleMovement()
    {
        paddle.SetDirection(InputManager.Instance.GetMovementDirection());
    }
}
