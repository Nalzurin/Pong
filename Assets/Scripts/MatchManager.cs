using UnityEngine;
using static Data;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance { get; private set; }
    [SerializeField] private GoalScript leftGoal;
    [SerializeField] private GoalScript rightGoal;
    [SerializeField] private GameObject ballSpawner;
    [SerializeField] private GameObject ballPrefab;

    private int _scoreLeft;
    public int ScoreLeft
    {
        get { return _scoreLeft; }
        private set
        {
            _scoreLeft = value;
            GUIManager.Instance.SetTextScoreLeft(value.ToString());

        }
    }
    private int _scoreRight;
    public int ScoreRight
    {
        get { return _scoreRight; }
        private set
        {
            _scoreRight = value;
            GUIManager.Instance.SetTextScoreRight(value.ToString());

        }
    }
    private float _timer;
    public float Timer
    {
        get { return _timer; }
        private set
        {
            _timer = value;
            GUIManager.Instance.SetTextTimer(((int)value).ToString());

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
    void Start()
    {
        leftGoal.SetPlayerSide(PlayerSide.Left);
        rightGoal.SetPlayerSide(PlayerSide.Right);
        SetUpMatch();
        BeginMatch();   
    }
    public void SetUpMatch()
    {
        Instance.ScoreLeft = 0;
        Instance.ScoreRight = 0;
        Timer = 90;

    }
    public void BeginMatch()
    {
        isMatchStarted = true;
        SpawnBall();
    }

    public void EndMatch()
    {
        Time.timeScale = 0;
    }
    public void SpawnBall()
    {
        Instantiate(ballPrefab, ballSpawner.transform.position, Quaternion.Euler(Vector2.zero));
    }
    private void Update()
    {
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
