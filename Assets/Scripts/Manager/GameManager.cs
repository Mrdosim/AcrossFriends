using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    public PlayerMove player;
    public AudioClip gameOverClip;

    private float score;
    private bool isGameOver;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        score = 0f;
        isGameOver = false;
        gameOverPanel.SetActive(false);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + score.ToString("0");
    }

    public void UpdateScore(float newScore)
    {
        if (!isGameOver && newScore > score)
        {
            score = newScore;
            UpdateScoreText();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        gameOverPanel.SetActive(true);
        SoundManager.Instance.PlayClip(gameOverClip); // 게임 오버 시 클립 재생
    }

    public void RestartGame()
    {
        gameOverPanel.SetActive(false);
        isGameOver = false;
        score = 0f;

        // MapManager의 InitializeMap 메서드를 호출하여 맵을 다시 생성
        MapManager.Instance.InitializeMap();
        player.ResetPosition();
        player.animator.SetBool("Hit",false);
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }
}
