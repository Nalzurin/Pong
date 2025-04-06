using UnityEngine;
using UnityEngine.UIElements;
using static Data;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance { get; private set; }
    [SerializeField] private GoalScript leftGoal;
    [SerializeField] private GoalScript rightGoal;
    [SerializeField] private GameObject ballSpawner;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject leftPaddle;
    [SerializeField] private GameObject rightPaddle;
    public BallScript Ball;
    public Difficulty difficulty;

    private bool doCountdown;
    private float _countdownTime;
    public float CountdownTime
    {
        get { return _countdownTime; }
        private set
        {
            _countdownTime = value;
            MatchGUIManager.Instance.SetCountDownTimer(((int)value).ToString());

        }
    }
    private int _scoreLeft;
    public int ScoreLeft
    {
        get { return _scoreLeft; }
        private set
        {
            _scoreLeft = value;
            MatchGUIManager.Instance.SetTextScoreLeft(value.ToString());

        }
    }
    private int _scoreRight;
    public int ScoreRight
    {
        get { return _scoreRight; }
        private set
        {
            _scoreRight = value;
            MatchGUIManager.Instance.SetTextScoreRight(value.ToString());

        }
    }
    private float _timer;
    public float Timer
    {
        get { return _timer; }
        private set
        {
            _timer = value;
            MatchGUIManager.Instance.SetTextTimer(((int)value).ToString());

        }
    }
    public bool isMatchStarted { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void SetUpMatch(int timeDuration, Difficulty _difficulty, int countdown, PlayerSide playerSide)
    {
        leftGoal.SetPlayerSide(PlayerSide.Left);
        rightGoal.SetPlayerSide(PlayerSide.Right);
        Instance.ScoreLeft = 0;
        Instance.ScoreRight = 0;
        Timer = timeDuration;
        difficulty = _difficulty;
        if(playerSide == PlayerSide.Left)
        {
            leftPaddle.AddComponent<PlayerController>();
            rightPaddle.AddComponent<BotController>();

        }
        else
        {
            rightPaddle.AddComponent<PlayerController>();
            leftPaddle.AddComponent<BotController>();

        }


        CountdownTime = countdown;
        doCountdown = true;
    }
    public void BeginMatch()
    {
        MatchGUIManager.Instance.SetCountDownTimer("");
        isMatchStarted = true;
        SpawnBall();
    }
    public void EndMatch()
    {
        Time.timeScale = 0;
    }
    public void SpawnBall()
    {
        Ball = (Instantiate(ballPrefab, ballSpawner.transform.position, Quaternion.Euler(Vector2.zero))).GetComponent<BallScript>();
    }
    private void Update()
    {
        if (doCountdown)
        {
            CountdownTime -= Time.deltaTime;
            if (CountdownTime <= 0)
            {
                doCountdown = false;
                MatchGUIManager.Instance.SetCountDownTimer(((int)CountdownTime).ToString());
                BeginMatch();
            }
        }
        if(isMatchStarted)
        {
            Timer -= Time.deltaTime;
            if(Timer <= 0)
            {
                EndMatch();
            }
        }
    }
    public void Score(PlayerSide side)
    {
        if (side == PlayerSide.Left)
        {
            ScoreRight++;
        }
        else
        {
            ScoreLeft++;
        }
        SpawnBall();
    }

}
