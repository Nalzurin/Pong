using TMPro;
using UnityEngine;
using static Data;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Instance { get; private set; }
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

    void Start()
    {
        textScoreLeft.text = string.Empty;
        textScoreRight.text = string.Empty;
        textTimer.text = string.Empty;

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
}
