using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button playButton;
    //public Transform paletteParent; // container with color buttons

    [Header("Audio Settings")]
    public AudioClip backgroundMusic; // Assign your mp3 in the Inspector
    private AudioSource audioSource;

    void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;   // Music loops by default
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

    void OnPlayClicked()
    {
        SceneManager.LoadScene("Level_01");
    }
}
