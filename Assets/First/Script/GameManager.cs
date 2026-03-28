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
    [Header("Cell Unlock (ตาม Wave)")]
    public GameObject neutrophilButton;  // link ปุ่มใน Inspector
    public GameObject macrophageButton;
    public GameObject nkCellButton;

// ไฟล์: GameManager.cs
    public void UpdateCellUnlock(int wave)
    {
        // Wave 1: มีแค่ Neutrophil
        neutrophilButton?.SetActive(true);

        // Wave 2 ขึ้นไป: ปลดล็อก Macrophage
        macrophageButton?.SetActive(wave >= 2); 

        // Wave 3 ขึ้นไป: ปลดล็อก NK Cell
        nkCellButton?.SetActive(wave >= 3);
    }

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
    // ATPManager.Instance?.OnKillBonus(); ← ลบบรรทัดนี้ออก
    WaveManager.Instance?.CheckWave(score);
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
        UpdateCellUnlock(WaveManager.Instance?.GetCurrentWave() ?? 1);

    }

    void GameOver()
    {
        Time.timeScale = 0f;
        BGMManager.Instance?.PlayGameOver();
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

    // ไฟล์: GameManager.cs (เพิ่มส่วน Victory)
    [Header("Victory UI")]
    public GameObject victoryPanel;
    public TextMeshProUGUI victoryStatusText;

    public void ShowVictory()
    {
        Time.timeScale = 0f; // หยุดเกม
        BGMManager.Instance?.PlayVictory();
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            if (victoryStatusText != null)
            {
                victoryStatusText.text = "";
            }
        }
    }

        public void GoToNextScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}