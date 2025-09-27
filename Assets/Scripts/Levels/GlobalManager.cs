using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance;

    [Header("Score Settings")]
    public int score = 0;
    public int maxScore = 5;

    [Header("UI References")]
    public TMP_Text scoreText;     // Link your score TMP text here
    public GameObject nextButton;  // Link your Next Button here
    public string next_level;

    [Header("Audio Settings")]
    public AudioClip backgroundMusic; 
    private AudioSource audioSource;

    void Awake()
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

    void Start()
    {
        UpdateScoreUI();
        if (nextButton != null) nextButton.SetActive(false);

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (backgroundMusic != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("No background music assigned in GlobalManager.");
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();

        if (score >= maxScore && nextButton != null)
        {
            nextButton.SetActive(true);
        }
    }

    public void RemoveScore(int amount)
    {
        score -= amount;
        if (score < 0) score = 0;
        UpdateScoreUI();

        if (nextButton != null && score < maxScore)
        {
            nextButton.SetActive(false); // Hide button if score drops below max
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score + " / " + maxScore;
        }
    }

    // Called by the Next Button OnClick
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(next_level);
    }
}
