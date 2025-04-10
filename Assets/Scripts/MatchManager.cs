using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] public GameObject topWall;
    [SerializeField] public GameObject bottomWall;
    public BallScript Ball;
    public Difficulty difficulty;
    public delegate void SomeoneScored();
    public event SomeoneScored someoneScored;
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
    public void QuitMatch()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }
    public void PauseMatch()
    {
        Time.timeScale = 0f;
    }
    public void UnpauseMatch()
    {
        Time.timeScale = 1f;
    }
    public void EndMatch()
    {
        Time.timeScale = 0;
        bool isDraw = ScoreLeft == ScoreRight;
        GameObject playerPaddle = leftPaddle.GetComponent<PlayerController>() != null ? leftPaddle : rightPaddle;
        GameObject winningSide = ScoreLeft > ScoreRight ? leftPaddle : rightPaddle;
        if (isDraw)
        {
            MatchGUIManager.Instance.EndMatch(2);
            return;
        }
        if(playerPaddle == winningSide)
        {
            MatchGUIManager.Instance.EndMatch(0);
        }
        else
        {
            MatchGUIManager.Instance.EndMatch(1);
        }
    }
    public void SpawnBall()
    {
        Random.InitState((int)Time.time);
        ballSpawner.transform.position = new Vector2(0, Random.Range(-9, 9));
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
        if(Ball != null)
        {
            if (Ball.transform.position.x > 22 || Ball.transform.position.x < -22)
            {
                Destroy(Ball);
                SpawnBall();
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
        someoneScored?.Invoke();
        SpawnBall();

    }

}
