using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static Data;
using Random = UnityEngine.Random;
[RequireComponent(typeof(PaddleController))]
public class BotController : MonoBehaviour
{
    public Difficulty difficulty;
    public PaddleController paddleController;
    [SerializeField] private float botXPosition;
    [SerializeField] private float topLimitY;
    [SerializeField] private float bottomLimitY;
    private float paddleReach = 1.5f;
    private float ballHalfSize = 0.25f;
    float effectiveTolerance;
    private float reactionCooldown;
    private float errorMargin;
    private float nextDecisionTime;
    private float currentTargetY;

    float skillBuffer => difficulty switch
    {
        Difficulty.Easy => -0.75f,
        Difficulty.Normal => -0.25f,
        Difficulty.Hard => 0f,
        _ => -0.25f,
    };
    float tolerance => Mathf.Max(0.1f, effectiveTolerance + skillBuffer); // Dead zone buffer


    void Start()
    {
        SetDifficultyParameters();
        currentTargetY = transform.position.y;
        botXPosition = transform.position.x - 0.75f/2;
        topLimitY = 9;
        bottomLimitY = -9;
        effectiveTolerance = paddleReach + ballHalfSize;
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
        if (Time.time <= nextDecisionTime)
        {
            return;
        }
        if (!IsBallApproaching(ballObj.transform.position, ballRb.linearVelocity))
        {
            paddleController.SetDirection(0f);
            return;
            
        }
        BotThink(ballRb, ballObj);
        nextDecisionTime = Time.time + reactionCooldown;
        
    }

    private void BotThink(Rigidbody2D ballRb, BallScript ballObj)
    {
        Vector2 predicted = PredictBallImpactPoint(ballObj.transform.position, ballRb.linearVelocity);
        currentTargetY = predicted.y + Random.Range(-errorMargin, errorMargin);
        float direction = Mathf.Sign(currentTargetY - transform.position.y);
        if (Mathf.Abs(currentTargetY - transform.position.y) > tolerance)
        {
            paddleController.SetDirection(direction);
        }
        else
        {
            paddleController.SetDirection(0f);
        }

    }

    void SetDifficultyParameters()
    {
        difficulty = MatchManager.Instance.difficulty;
        switch (difficulty)
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
            {
                velocity.y *= -1;
                velocity *= 1.1f;
            }


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

