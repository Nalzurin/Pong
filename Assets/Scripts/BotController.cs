using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Data;

[RequireComponent(typeof(PaddleController))]
public class BotController : MonoBehaviour
{
    private PaddleController paddle;
    private BoxCollider2D boxCollider;
    private Difficulty difficulty = Difficulty.Normal;
    [SerializeField] private float hardTimeOffset = 0.2f;
    private float variableTimeOffset = 0.3f;
    private float tickFast = 0.2f;
    private float currentTick = 0;
    public void SetDifficulty(Difficulty newDifficulty)
    {
        difficulty = newDifficulty;
    }
    private void Awake()
    {
        paddle = GetComponent<PaddleController>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        Debug.Log(MatchManager.Instance.difficulty);
        SetDifficulty(MatchManager.Instance.difficulty);
    }
    private void Update()
    {
        if (difficulty == Difficulty.Easy)
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
        if (Vector3.Distance(CalculateBallPosition(), transform.position) < 5f)
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
        if (Vector3.Distance(CalculateBallPosition(), transform.position) < 5f)
        {
            paddle.SetDirection(GetDirection(CalculateBallPosition()));
        }
        else
        {
            paddle.SetDirection(GetDirection(CalculateBallPositionAfterTimeAccountForBounce(variableTimeOffset)));
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
        if(!WillBallBounceAtVector(start, future, out timeLeft, out newVector, time))
        {
            return future;
        }
        else
        {
            Vector3 newVelocity = Vector3.Reflect(MatchManager.Instance.Ball.rb.linearVelocity, Vector3.up);
            variableTimeOffset = Mathf.Min(variableTimeOffset -0.03f, 0.05f);
            return CalculateVectorPositionAfterTime(newVector, newVelocity, timeLeft);
        }

    }

    private bool WillBallBounceAtVector(Vector3 start, Vector3 future, out float timeLeft, out Vector3 newVector, float time = -1f)
    {
        float distance = Vector3.Distance(start, future);
        float speed = 0f;
        if (time != -1)
        {
            speed = distance / time;
        }

        RaycastHit hit;
        
        if(Physics.BoxCast(start, MatchManager.Instance.Ball.boxCollider.bounds.extents, MatchManager.Instance.Ball.rb.linearVelocity.normalized, out hit, MatchManager.Instance.Ball.transform.rotation, distance))
        {
            if(hit.collider.CompareTag(WALLTAG))
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
