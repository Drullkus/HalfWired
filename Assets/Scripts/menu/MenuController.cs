using UnityEngine;

public class MenuController : MonoBehaviour
{
    
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject leaderBoard;

    private void HideAll()
    {
        mainMenu.SetActive(false);
        options.SetActive(false);
        credits.SetActive(false);
        gameOver.SetActive(false);
        leaderBoard.SetActive(false);
    }

    public void EnterMainMenu()
    {
        HideAll();
        mainMenu.SetActive(true);
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
        gameOver.SetActive(true);
    }

    public void EnterLeaderBoard()
    {
        HideAll();
        leaderBoard.SetActive(true);
    }

}
