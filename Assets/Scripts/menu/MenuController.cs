using TMPro;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private FirstPersonController player;
    
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject leaderBoard;
    
    [SerializeField] private HudController hudController;
    [SerializeField] private GameObject highScoreField;

    [SerializeField] private TMP_Text leaderBoardText;

    private void HideAll()
    {
        GameMaster.Instance.MenuOpen = true;
        mainMenu.SetActive(false);
        options.SetActive(false);
        credits.SetActive(false);
        gameOver.SetActive(false);
        leaderBoard.SetActive(false);
    }

    public void EnterGame()
    {
        HideAll();
        Debug.Log("Start Game");
        GameMaster.Instance.MenuOpen = false;
    }

    public void ExitGame()
    {
        HideAll();
        Debug.Log("Exit Game");
    }

    public void EnterMainMenu()
    {
        HideAll();
        mainMenu.SetActive(true);
        hudController.Reset();
    }

    public void EnterOptions()
    {
        HideAll();
        options.SetActive(true);
    }

    public void EnterCredits()
    {
        HideAll();
        credits.SetActive(true);
    }

    public void EnterGameOver()
    {
        HideAll();

        highScoreField.SetActive(GameMaster.Instance.IsNewHighScore(hudController.hits));
        gameOver.SetActive(true);
    }

    public void EnterLeaderBoard()
    {
        HideAll();
        leaderBoardText.text = GameMaster.Instance.GetScoresString();
        leaderBoard.SetActive(true);
    }

}
