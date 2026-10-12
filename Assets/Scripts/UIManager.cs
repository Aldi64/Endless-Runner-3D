using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;

    // SetText with a format argument avoids creating garbage strings every update,
    // which matters on mobile.
    public void SetScore(int score) => scoreText.SetText("Score: {0}", score);

    public void ShowGameOver(int score)
    {
        finalScoreText.SetText("Score: {0}", score);
        gameOverPanel.SetActive(true);
    }

    public void HideGameOver() => gameOverPanel.SetActive(false);
}