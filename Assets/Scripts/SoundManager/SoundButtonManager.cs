using UnityEngine;
using UnityEngine.UI;

public class SoundButtonManager : MonoBehaviour
{
    private const string PlayerPrefsKey = "BackgroundMusicEnabled";

    [SerializeField]
    AudioSource audioSource;
    private bool isMusicEnabled;

    [SerializeField]
    Button buttonVolume;
    [SerializeField]
    Image iconState;
    [SerializeField]
    Sprite spriteVolumeOn;
    [SerializeField]
    Sprite spriteVolumeOff;

    [Header("---------- SFX Sound ----------")]
    [SerializeField]
    AudioSource sfxSource;
    public AudioClip clickButton;
    public AudioClip success;
    public AudioClip failed;
    public AudioClip select_item;
    public AudioClip digging;
    public AudioClip watering;
    public AudioClip planting;
    public AudioClip picking_up_plant;

    private void Awake()
    {
        buttonVolume.onClick.AddListener(ToggleMusic);
    }

    private void Start()
    {
        isMusicEnabled = PlayerPrefs.GetInt(PlayerPrefsKey, 1) == 1;

        if (isMusicEnabled)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

        iconState.sprite = isMusicEnabled ? spriteVolumeOn : spriteVolumeOff;
    }

    public void ToggleMusic()
    {
        PlaySFX(clickButton);
        isMusicEnabled = !isMusicEnabled;
        PlaySFX(clickButton);

        PlayerPrefs.SetInt(PlayerPrefsKey, isMusicEnabled ? 1 : 0);
        PlayerPrefs.Save();

        if (isMusicEnabled)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }

        iconState.sprite = isMusicEnabled ? spriteVolumeOn : spriteVolumeOff;
    }

    public void PlaySFX(AudioClip clip)
    {
        if (isMusicEnabled) 
        {
            sfxSource.PlayOneShot(clip);
        }
    }
}
