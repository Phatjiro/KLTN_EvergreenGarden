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

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        buttonVolume.onClick.AddListener(SwitchVolumeState);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource.Play();
        imageVolumeOff.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchVolumeState()
    {
        if (m_AudioSource.isPlaying)
        {
            m_AudioSource.Stop();
            imageVolumeOn.enabled = false;
            imageVolumeOff.enabled = true;
        }
        else
        {
            m_AudioSource.Play();
            imageVolumeOff.enabled = false;
            imageVolumeOn.enabled = true;
        }
    }
}
