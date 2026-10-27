using TMPro;
using UnityEngine;

public class GameOverHighScoreController : MonoBehaviour
{
    
    [SerializeField] private HudController hudController;
    [SerializeField] private GameObject highScoreObjects;
    [SerializeField] private TMP_InputField highScoreField;
    
    public void OnClick()
    {
        if (highScoreField.text.Length == 0) return;

        Debug.Log($"Submitting highscore {hudController.hits} for '{highScoreField.text}'");
        
        GameMaster.Instance.AddScore(highScoreField.text, hudController.hits);
        
        highScoreObjects.SetActive(false);

        highScoreField.text = "";
    }
    
}
