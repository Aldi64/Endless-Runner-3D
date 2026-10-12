using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string BestScoreKey = "BestScore";

    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private UIManager ui;

    [Header("Speed")]
    [SerializeField] private float baseSpeed = 10f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float speedGainPerSecond = 0.2f;

    [Header("Score")]
    [SerializeField] private int pointsPerCoin = 10;

    private float elapsed;
    private int lastScore = -1;

    public int Coins { get; private set; }
    public int BestScore { get; private set; }
    public int Score => Mathf.FloorToInt(player.transform.position.z) + Coins * pointsPerCoin;
    public bool IsGameOver { get; private set; }

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 60;
        BestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    private void Start()
    {
        player.SetForwardSpeed(baseSpeed);
        ui.HideGameOver();
    }

    private void Update()
    {
        if (IsGameOver)
        {
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
                Restart();
            return;
        }

        elapsed += Time.deltaTime;
        player.SetForwardSpeed(Mathf.Min(baseSpeed + speedGainPerSecond * elapsed, maxSpeed));

        int score = Score;
        if (score != lastScore)
        {
            lastScore = score;
            ui.SetScore(score);
        }
    }

    public void AddCoin()
    {
        if (IsGameOver) return;
        Coins++;

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayCoin();
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        player.enabled = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOver();
            AudioManager.Instance.StopMusic();
        }

        int finalScore = Score;
        bool isNewBest = finalScore > BestScore;
        if (isNewBest)
        {
            BestScore = finalScore;
            PlayerPrefs.SetInt(BestScoreKey, BestScore);
            PlayerPrefs.Save();
        }

        ui.ShowGameOver(finalScore, BestScore, isNewBest);
    }

    public void Restart() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    // Handy for testing: use the three-dot menu on this component in the Inspector.
    [ContextMenu("Reset Best Score")]
    private void ResetBestScore()
    {
        PlayerPrefs.DeleteKey(BestScoreKey);
        BestScore = 0;
    }
}