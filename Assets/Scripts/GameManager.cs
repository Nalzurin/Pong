using UnityEngine;
using UnityEngine.SceneManagement;
using static Data;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] public int matchDuration = 90;
    [SerializeField] public PlayerSide playerSide = PlayerSide.Left;
    [SerializeField] public int countDown = 3;

    public Difficulty aiDifficulty = Difficulty.Easy;

    public void SwitchAIDifficulty(Difficulty newDifficulty)
    {
        aiDifficulty = newDifficulty;
    }
    public void SwitchAIDifficulty(int newDifficulty)
    {

        switch (newDifficulty)
        {
            case 0:
                aiDifficulty = Difficulty.Easy;
                break;
            case 1:
                aiDifficulty = Difficulty.Normal;
                break;
            case 2:
                aiDifficulty = Difficulty.Hard;
                break;
            default:
                aiDifficulty = Difficulty.Normal;
                break;
        }
    }

    public async void LoadMatch()
    {
        await SceneManager.LoadSceneAsync("Game");
        MatchManager.Instance.SetUpMatch(matchDuration, aiDifficulty, countDown, playerSide);
    }
    public async void UnloadMatch()
    {
        await SceneManager.LoadSceneAsync("MainMenu");
        MainMenuGUI.Instance.ChangeState(new MainMenuStateComponent() { state = Data.MenuState.Main });
        aiDifficulty = Difficulty.Easy;
        matchDuration = 90;
        countDown = 3;
        playerSide = PlayerSide.Left;
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        
    }
}
