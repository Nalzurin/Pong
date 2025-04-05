using UnityEngine;

public class MatchManager : MonoBehaviour
{
    private static MatchManager instance;

    private int scoreLeft;
    private int scoreRight;
    public int ScoreLeft => scoreLeft;
    public int ScoreRight => scoreRight;

    public MatchManager GetInstance()
    {
        return instance;
    }
    
    MatchManager()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance.scoreLeft = 0;
        instance.scoreRight = 0;
    }

    public void Score(bool leftSide)
    {
        if (leftSide)
        {
            scoreLeft++;
        }
        else
        {
            scoreRight++;
        }
    }

}
