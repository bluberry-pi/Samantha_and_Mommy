using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DownloadManager : MonoBehaviour
{
    public Slider downloadSlider;
    public TextMeshProUGUI speedText;

    [Header("Download Speeds")]
    public float firstHalfSpeed = 5f;
    public float secondHalfSpeed = 0.5f;

    [HideInInspector] public float shakeBoost = 0f;

    float currentProgress = 0f;

    void Update()
    {
        if (currentProgress >= 1f)
        {
            currentProgress = 1f;
            downloadSlider.value = 1f;
            speedText.text = "Download Complete";
            return;
        }

        float currentSpeed;

        if (currentProgress < 0.10f)
            currentSpeed = firstHalfSpeed;
        else
            currentSpeed = secondHalfSpeed;

        float finalSpeed = currentSpeed + shakeBoost;
        currentProgress += finalSpeed * Time.deltaTime / 100f;

        downloadSlider.value = currentProgress;
        speedText.text = finalSpeed.ToString("F2") + " kb/s";
    }
}