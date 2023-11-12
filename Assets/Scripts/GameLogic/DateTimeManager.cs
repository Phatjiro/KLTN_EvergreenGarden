using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DateTimeManager : MonoBehaviour
{
    [SerializeField]
    private Text textCurrentTime;
    private int timeMultiplier = 8;
    [SerializeField] Gradient gradient;
    private Light2D lightForMap;

    [SerializeField]
    Text textCurrentState;
    [SerializeField]
    Button buttonOnOfEffect;

    static string KEY_DayLifeEffectState = "DayLifeEffectState";

    private void Awake()
    {
        buttonOnOfEffect.onClick.AddListener(TurnOnOffEffect);
    }

    // Start is called before the first frame update
    void Start()
    {
        lightForMap = GetComponent<Light2D>();
        if (PlayerPrefs.GetString(KEY_DayLifeEffectState) == "ON" || PlayerPrefs.GetString(KEY_DayLifeEffectState) == "")
        {
            TurnOnOffEffect(true);
        }
        else
        { 
            TurnOnOffEffect(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        int seconds = ConvertCurrentTimeToSeconds(timeMultiplier);
        string timeDisplay = ConvertSecondsToTime(seconds);
        int hours = ConvertSecondsToHours(seconds);
        // Format DateTime
        //string timeFormat = scaledTime.ToString("HH:mm");

        textCurrentTime.text = timeDisplay;

        int minute = ConvertSecondsToMinute(seconds);
        int totalminutes = minute + hours * 60;
        controlColor(totalminutes);
    }

    public int ConvertCurrentTimeToSeconds(int timeMultiplier)
    {
        // Get current time
        DateTime currentTime = DateTime.Now;

        int seconds = currentTime.Hour * 3600 + currentTime.Minute * 60 + currentTime.Second;
        seconds = (seconds * timeMultiplier) % 86400;
        return seconds;
    }

    private string ConvertSecondsToTime(int totalSeconds)
    {
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;

        string timeFormat = string.Format("{0:00}:{1:00}", hours, minutes);

        return timeFormat;
    }
    public int ConvertSecondsToHours(int totalSeconds)
    {
        int hours = totalSeconds / 3600;
        return hours;
    }
    private int ConvertSecondsToMinute(int totalSeconds)
    {
        int minutes = (totalSeconds % 3600) / 60;
        return minutes;
    }
    private void controlColor(int totalminute)
    {
        if (textCurrentState.text == "ON")
        {
            lightForMap.color = gradient.Evaluate(percentOfDay(totalminute));
        }
    }

    // totalminute = minute + hours * 60
    //1440 = 24*60
    private float percentOfDay(int totalminute)
    {
        return (float)totalminute / 1440;
    }

    public void TurnOnOffEffect()
    {
        if (textCurrentState.text == "ON")
        {
            lightForMap.enabled = false;
            textCurrentState.text = "OFF";
        }
        else
        {
            lightForMap.enabled = true;
            textCurrentState.text = "ON";
        }
        PlayerPrefs.SetString(KEY_DayLifeEffectState, textCurrentState.text);
    }

    public void TurnOnOffEffect(bool state)
    {
        if (state == true)
        {
            lightForMap.enabled = true;
            textCurrentState.text = "ON";
        }
        else
        {
            lightForMap.enabled = false;
            textCurrentState.text = "OFF";
        }
    }
}