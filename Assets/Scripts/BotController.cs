using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Data;

[RequireComponent(typeof(PaddleController))]
public class BotController : MonoBehaviour
{
    private PaddleController paddle;

    private Difficulty difficulty = Difficulty.Normal;
    [SerializeField] private float hardTimeOffset = 0.2f;
    public float variableTimeOffset = 1f;
    private float tickFast = 0.1f;
    private float currentTick = 0;
    private bool doTick = true;
    public void SetDifficulty(Difficulty newDifficulty)
    {
        difficulty = newDifficulty;
    }
    private void Awake()
    {
        paddle = GetComponent<PaddleController>();
    }
    private void Start()
    {
        Debug.Log(MatchManager.Instance.difficulty);
        SetDifficulty(MatchManager.Instance.difficulty);
        MatchManager.Instance.someoneScored += ResetVariableTime;
    }
    private void Update()
    {
        if (doTick)
        {
            currentTick += Time.deltaTime;
            if (currentTick < tickFast)
            {
                return;
            }
            currentTick = 0f;
        }
        if (MatchManager.Instance == null)
        {
            return;
        }
        if (!MatchManager.Instance.isMatchStarted)
        {
            return;
        }
        HandleMovement();

    }
    private void HandleMovement()
    {
        if (MatchManager.Instance.Ball == null)
        {
            return;
        }
        //Debug.Log(difficulty);
        switch (difficulty)
        {
            case Difficulty.Easy:
                HandleMovementEasy();
                break;
            case Difficulty.Normal:
                HandleMovementNormal();
                break;
            case Difficulty.Hard:
                HandleMovementHard();
                break;
        }
    }
    private void HandleMovementEasy()
    {
        paddle.SetDirection(GetDirection(CalculateBallPosition()));
    }
    private void HandleMovementNormal()
    {
        if (Mathf.Abs(transform.position.x - CalculateBallPosition().x) < 4f)
        {
            paddle.SetDirection(GetDirection(CalculateBallPosition()));
        }
        else
        {
            paddle.SetDirection(GetDirection(CalculateBallPositionAfterTime(hardTimeOffset)));
        }

    }
    private void HandleMovementHard()
    {
        if (Mathf.Abs(transform.position.x - CalculateBallPosition().x) < 4f)
        {
            paddle.SetDirection(GetDirection(CalculateBallPosition()));
        }
        else
        {
            paddle.SetDirection(GetDirection(CalculateBallPositionAfterTimeAccountForBounce(0.01f)));
        }
    }
    private float GetDirection(Vector3 targetPosition)
    {
        if (transform.position.y< targetPosition.y)
        {
            return 1f;
        }
        else
        {
            return -1f;
        }
    }
    private Vector3 CalculateBallPosition()
    {
        if (MatchManager.Instance.Ball == null)
        {
            return Vector3.zero;
        }

        return MatchManager.Instance.Ball.transform.position;
    }
    private void ResetVariableTime()
    {
        variableTimeOffset = 1f;
    }
    private Vector3 CalculateBallPositionAfterTime(float time)
    {
        if (MatchManager.Instance.Ball == null)
        {
            return Vector3.zero;
        }
        Vector3 start = CalculateBallPosition();
        Vector3 future = CalculateVectorPositionAfterTime(start, MatchManager.Instance.Ball.rb.linearVelocity, time);
        return future;
    }
    private Vector3 CalculateBallPositionAfterTimeAccountForBounce(float time)
    {
        if (MatchManager.Instance.Ball == null)
        {
            return Vector3.zero;
        }
        Vector3 start = CalculateBallPosition();
        Vector3 future = CalculateBallPositionAfterTime(time);
        float timeLeft;
        Vector3 newVector;
        if(!WillBallBounceAtVector(start, MatchManager.Instance.Ball.rb.linearVelocity, out timeLeft, out newVector, 1f))
        {
            return future;
        }
        else
        {
            
            Vector3 newVelocity = Vector3.Reflect(MatchManager.Instance.Ball.rb.linearVelocity, Vector3.up);
            variableTimeOffset = Mathf.Max(variableTimeOffset -0.025f, 0.05f);
            Debug.DrawRay(newVector, newVector + (newVelocity * time), Color.white, 1);
            return CalculateVectorPositionAfterTime(newVector, newVelocity, time);
        }

    }

    private bool WillBallBounceAtVector(Vector3 start, Vector3 velocity, out float timeLeft, out Vector3 newVector, float time)
    {
        float distance = (velocity * time).magnitude;
        float speed = 0f;
        if (time != -1)
        {
            speed = distance / time;
        }
        RaycastHit2D hit = Physics2D.Raycast(start, velocity.normalized, distance, LayerMask.GetMask("Walls"));
        if (hit == true)
        {
            Debug.Log("Will collide");
            Debug.Log(hit.collider.tag);
            Debug.DrawRay(start, hit.point, Color.white, 1);

            if (hit.collider.CompareTag(WALLTAG))
            {
                newVector = hit.point;
                timeLeft = time - (hit.distance / speed);
                return true;
            }
        }
        newVector = Vector3.zero;
        timeLeft = 0f;
        return false;
    }
    private Vector3 CalculateVectorPositionAfterTime(Vector3 start, Vector3 velocity, float time)
    {
        return start + (velocity * time);

    }
}
