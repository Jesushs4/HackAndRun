using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] levelClips;
    [SerializeField] private AudioSource hackCorrect;
    [SerializeField] private AudioSource hackFail;
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource hurtSound;


    private string currentScene = "";


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            //assign the static instance to the object
            Instance = this;
            //maintain this instance to the next scene
            DontDestroyOnLoad(this.gameObject);
        }

        PlayMusicLevel(levelClips[SceneManager.GetActiveScene().buildIndex]);

    }

    private void Update()
    {
        
        if (currentScene != "")
        {
            if (currentScene != SceneManager.GetActiveScene().name) {
                    PlayMusicLevel(levelClips[SceneManager.GetActiveScene().buildIndex]);
                
            }
        }
        currentScene = SceneManager.GetActiveScene().name;
    }


    private void PlayMusicLevel(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void HackCorrect()
    {
        hackCorrect.Play();
    }

    public void HackFail()
    {
        hackFail.Play();
    }

    public void Dash()
    {
        dashSound.Play();
    }

    public void Jump() { jumpSound.Play(); }

    public void Hurt () { hurtSound.Play(); }



}