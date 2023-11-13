using UnityEngine;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    [SerializeField]
    AudioSource m_AudioSource;
    [SerializeField]
    Button buttonVolume;
    [SerializeField]
    Image imageVolumeOn;
    [SerializeField]
    Image imageVolumeOff;

    static string SOUND_PREF_KEY = "SoundStatus";

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        buttonVolume.onClick.AddListener(SwitchVolumeState);
    }
    // Start is called before the first frame update
    void Start()
    {
        int soundStatus = PlayerPrefs.GetInt(SOUND_PREF_KEY, 1);

        if (soundStatus == 0)
        {
            TurnOffAudio();
            imageVolumeOff.enabled = true;
            imageVolumeOn.enabled = false;
        }
        else
        {
            TurnOnAudio();
            imageVolumeOff.enabled = false;
            imageVolumeOn.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchVolumeState()
    {
        if (PlayerPrefs.GetInt(SOUND_PREF_KEY, 1) == 1)
        {
            TurnOffAudio();
            imageVolumeOn.enabled = false;
            imageVolumeOff.enabled = true;
            PlayerPrefs.SetInt(SOUND_PREF_KEY, 0);
        }
        else
        {
            TurnOnAudio();
            imageVolumeOff.enabled = false;
            imageVolumeOn.enabled = true;
            PlayerPrefs.SetInt(SOUND_PREF_KEY, 1);
        }
    }

    public void TurnOffAudio()
    {
        m_AudioSource.Stop();
    }

    public void TurnOnAudio()
    {
        m_AudioSource.Play();
    }
}
