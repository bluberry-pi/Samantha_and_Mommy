using UnityEngine;
using TMPro;

public class NightClock : MonoBehaviour
{
    [Header("Clock Display")]
    public TextMeshProUGUI timeText;

    [Header("Clock Speed")]
    public float timeSpeed = 60f;   // How fast time runs (60 = 1 real second = 1 in-game minute)

    [Header("Night Range")]
    public int startHour = 22; // 10 PM
    public int endHour = 3;    // 3 AM

    float currentMinutes;

    void Start()
    {
        currentMinutes = startHour * 60;   // Convert 10:00 PM to minutes
    }

    void Update()
    {
        if (SceneBeginning.CutsceneActive) return;
        currentMinutes += Time.deltaTime * timeSpeed;

        int endMinutes = (24 + endHour) * 60;   // 27:00 → 3AM

        if (currentMinutes >= endMinutes)
        {
            currentMinutes = endMinutes;   // Stop at 3 AM (change to loop if needed)
        }

        UpdateClockUI();
    }

    void UpdateClockUI()
    {
        int totalMinutes = Mathf.FloorToInt(currentMinutes);
        int hour = (totalMinutes / 60) % 24;
        int minute = totalMinutes % 60;

        bool isPM = hour >= 12;
        int displayHour = hour % 12;
        if (displayHour == 0) displayHour = 12;

        string ampm = isPM ? "PM" : "AM";
        timeText.text = displayHour + ":" + minute.ToString("00") + " " + ampm;
    }
}