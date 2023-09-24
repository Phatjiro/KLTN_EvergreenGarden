using System;
using UnityEngine;
using UnityEngine.UI;

public class DateTimeManager : MonoBehaviour
{
    [SerializeField]
    private Text textCurrentTime;
    private int timeMultiplier = 8;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int seconds = ConvertCurrentTimeToSeconds(timeMultiplier);
        string timeDisplay = ConvertSecondsToTime(seconds);

        // Format DateTime
        //string timeFormat = scaledTime.ToString("HH:mm");

        textCurrentTime.text = timeDisplay;
    }

    private int ConvertCurrentTimeToSeconds(int timeMultiplier)
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
}
