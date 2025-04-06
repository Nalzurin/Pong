using UnityEngine;
using UnityEngine.SceneManagement;
using static Data;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }


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
        await SceneManager.LoadSceneAsync("Game", LoadSceneMode.Additive);
        MainMenuGUI.Instance.gameObject.SetActive(false);
        MatchManager.Instance.SetUpMatch(90, aiDifficulty, 3, PlayerSide.Left);
    }
    public void UnloadMatch()
    {
        SceneManager.UnloadSceneAsync("Game");
        MainMenuGUI.Instance.gameObject.SetActive(true);
        MainMenuGUI.Instance.ChangeState(new MainMenuStateComponent() { state = Data.MenuState.Main });

    }

    public void CloseGame()
    {
        Application.Quit();
    }

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
}
