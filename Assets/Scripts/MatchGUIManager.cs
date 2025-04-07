using TMPro;
using UnityEngine;
using static Data;

public class MatchGUIManager : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    public static MatchGUIManager Instance { get; private set; }
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
        canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }

    [SerializeField] private TMP_Text textScoreLeft;
    [SerializeField] private TMP_Text textScoreRight;
    [SerializeField] private TMP_Text textTimer;
    [SerializeField] private TMP_Text countDownTimer;
    void Start()
    {
        textScoreLeft.text = "0";
        textScoreRight.text = "0";
        textTimer.text = "0";
    }
    public void SetTextScoreLeft(string text)
    {
        textScoreLeft.text = text;
    }
    public void SetTextScoreRight(string text)
    {
        textScoreRight.text = text;
    }
    public void SetTextTimer(string text)
    {
        textTimer.text = text;
    }
    public void SetCountDownTimer(string text)
    {
        countDownTimer.text = text;
    }
}
