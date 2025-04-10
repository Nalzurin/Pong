using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Data;

public class MatchGUIManager : MonoBehaviour
{
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
    }

    [SerializeField] private TMP_Text textScoreLeft;
    [SerializeField] private TMP_Text textScoreRight;
    [SerializeField] private TMP_Text textTimer;
    [SerializeField] private TMP_Text countDownTimer;
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject FinishMenu;
    [SerializeField] private TMP_Text endGameText;
    void Start()
    {
        textScoreLeft.text = "0";
        textScoreRight.text = "0";
        textTimer.text = "0";
    }
    public void EndMatch(int state)
    {
        FinishMenu.SetActive(true);
        switch (state)
        {
            case 0:
                endGameText.text = "You Win!";
                break;
            case 1:
                endGameText.text = "You Win!";
                break;
            case 2:
                endGameText.text = "Draw!";
                break;
        }
    }
    public void PauseMatch()
    {
        PauseMenu.SetActive(true);
        MatchManager.Instance.PauseMatch();
    }
    public void UnpauseMatch()
    {
        PauseMenu.SetActive(false);
        MatchManager.Instance.UnpauseMatch();
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
