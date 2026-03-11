using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stats")]
    public int score     = 0;
    public int killCount = 0;
    public int hp        = 3;
    public int maxHP     = 3;

    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killText;
    public TextMeshProUGUI waveText;

    [Header("HP Hearts")]
    public Image[] heartImages;
    public Sprite  heartFull;
    public Sprite  heartEmpty;

    [Header("Game Over")]
    public GameObject      gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        WaveManager.Instance?.CheckWave(score);
    }

    public void AddScore(int points = 10)
    {
        score += points;
        killCount++;
        ATPManager.Instance?.OnKillBonus();
        WaveManager.Instance?.CheckWave(score);
        CytokineSpawner.Instance?.OnEnemyKilled();
        UpdateUI();
    }

    public void TakeDamage(int damage = 1)
    {
        hp = Mathf.Max(hp - damage, 0);
        UpdateUI();
        if (hp <= 0) GameOver();
    }

    public void Heal(int amount = 1)
    {
        hp = Mathf.Min(hp + amount, maxHP);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (killText  != null) killText.text  = "Kills: " + killCount;
        if (waveText  != null && WaveManager.Instance != null)
            waveText.text = "Wave: " + WaveManager.Instance.GetCurrentWave();

        for (int i = 0; i < heartImages.Length; i++)
            if (heartImages[i] != null)
                heartImages[i].sprite = (i < hp) ? heartFull : heartEmpty;
    }

    void GameOver()
    {
        Time.timeScale = 0f;
        if (gameOverPanel  != null) gameOverPanel.SetActive(true);
        if (finalScoreText != null) finalScoreText.text = $"Score: {score}\nKills: {killCount}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}