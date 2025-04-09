using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Data;

[RequireComponent(typeof(PaddleController))]
public class BotController : MonoBehaviour
{
    public Difficulty difficulty;
    public PaddleController paddleController;
    [SerializeField] private float botXPosition;
    [SerializeField] private float topLimitY;
    [SerializeField] private float bottomLimitY;

    private float reactionCooldown;
    private float errorMargin;
    private float nextDecisionTime;
    private float currentTargetY;

    void Start()
    {
        SetDifficultyParameters();
        currentTargetY = transform.position.y;
        botXPosition = transform.position.x;
        topLimitY = MatchManager.Instance.topWall.transform.position.y;
        bottomLimitY = MatchManager.Instance.bottomWall.transform.position.y;
        paddleController = GetComponent<PaddleController>();
    }

    void FixedUpdate()
    {
        BallScript ballObj = MatchManager.Instance?.Ball;

        if (ballObj == null)
        {
            paddleController.SetDirection(0f); // No ball = do nothing
            return;
        }

        Rigidbody2D ballRb = ballObj.GetComponent<Rigidbody2D>();
        if (IsBallApproaching(ballObj.transform.position, ballRb.linearVelocity))
        {
            if (Time.time >= nextDecisionTime)
            {
                Vector2 predicted = PredictBallImpactPoint(ballObj.transform.position, ballRb.linearVelocity);
                currentTargetY = predicted.y + Random.Range(-errorMargin, errorMargin);
                nextDecisionTime = Time.time + reactionCooldown;
            }

            float direction = Mathf.Sign(currentTargetY - transform.position.y);
            if (Mathf.Abs(currentTargetY - transform.position.y) > 0.1f)
            {
                paddleController.SetDirection(direction);
            }
            else
            {
                paddleController.SetDirection(0f);
            }
        }
        else
        {
            paddleController.SetDirection(0f);
        }
    }

    void SetDifficultyParameters()
    {
        switch (MatchManager.Instance.difficulty)
        {
            case Difficulty.Easy:
                reactionCooldown = 0.7f;
                errorMargin = 2.5f;
                break;

            case Difficulty.Normal:
                reactionCooldown = 0.3f;
                errorMargin = 1.5f;
                break;

            case Difficulty.Hard:
                reactionCooldown = 0f;
                errorMargin = 0f;
                break;
        }
    }

    bool IsBallApproaching(Vector2 ballPos, Vector2 ballVelocity)
    {
        return (transform.position.x < ballPos.x && ballVelocity.x < 0f) ||
               (transform.position.x > ballPos.x && ballVelocity.x > 0f);
    }

    Vector2 PredictBallImpactPoint(Vector2 pos, Vector2 velocity)
    {
        float simulatedTime = 0f;

        while ((velocity.x > 0 && pos.x < botXPosition) || (velocity.x < 0 && pos.x > botXPosition))
        {
            pos += velocity * Time.fixedDeltaTime;
            simulatedTime += Time.fixedDeltaTime;

            if (pos.y >= topLimitY || pos.y <= bottomLimitY)
                velocity.y *= -1;

            if (simulatedTime > 5f)
            {
                break;
            }
        }

        float difficultyBias = Random.Range(-errorMargin, errorMargin);

        if (velocity.magnitude > 10f)
        {
            difficultyBias *= 1.5f;

        }

        return new Vector2(pos.x, pos.y + difficultyBias);
    }
}

