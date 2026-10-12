using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text finalScoreText;
    [SerializeField] private TMP_Text bestScoreText;

    public void SetScore(int score) => scoreText.SetText("Score: {0}", score);

    public void ShowGameOver(int score, int best, bool isNewBest)
    {
        finalScoreText.SetText("Score: {0}", score);

        if (isNewBest)
            bestScoreText.SetText("New best!");
        else
            bestScoreText.SetText("Best: {0}", best);

        gameOverPanel.SetActive(true);
    }

    public void HideGameOver() => gameOverPanel.SetActive(false);
}