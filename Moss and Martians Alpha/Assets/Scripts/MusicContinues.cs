using UnityEngine;
using UnityEngine.UI;

public class MusicContinues : MonoBehaviour 
{
    private static MusicContinues instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private GameObject sliderRoot;
    private static bool placed = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    private void Start()
    {
        if (placed == false)
        {
            sliderRoot.SetActive(true);
            placed = true;
        }
        if (backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }
        if (musicSlider != null)
        {
            musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
            
        }
        //musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });
    }



    public static void SetVolume(float volume)
    {
        instance.audioSource.volume = volume;
    }
    public void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
        }
        if (audioSource.clip != null)
        {
            if (resetSong)
            {
                audioSource.Stop();
            }
            audioSource.Play();
        }
    }
}
