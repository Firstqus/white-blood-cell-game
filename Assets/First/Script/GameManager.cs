using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Stats")]
    public int score = 0;
    public int killCount = 0;
    public int hp = 3;
    public int maxHP = 3;

    [Header("UI Text")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI killText;

    [Header("HP Hearts")]
    public Image[] heartImages;   // ลาก Image หัวใจทั้ง 3 ใส่ตรงนี้
    public Sprite heartFull;      // รูปหัวใจเต็ม
    public Sprite heartEmpty;     // รูปหัวใจว่าง

    [Header("Game Over")]
    public GameObject gameOverPanel;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void AddScore(int points = 10)
    {
        score += points;
        killCount++;
        UpdateUI();
    }

    public void TakeDamage(int damage = 1)
    {
        hp -= damage;
        hp = Mathf.Max(hp, 0);
        UpdateUI();

        if (hp <= 0)
            GameOver();
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (killText  != null) killText.text  = "Kills: " + killCount;

        // อัปเดตหัวใจแต่ละดวง
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
                heartImages[i].sprite = (i < hp) ? heartFull : heartEmpty;
        }
    }

    void GameOver()
    {
        Time.timeScale = 0f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
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