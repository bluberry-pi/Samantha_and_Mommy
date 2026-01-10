using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DownloadManager : MonoBehaviour
{
    public Slider downloadSlider;
    public TextMeshProUGUI speedText;

    public float firstHalfSpeed = 5f;
    public float secondHalfSpeed = 0.5f;

    [HideInInspector] public float shakeBoost = 0f;

    float currentProgress = 0f;
    bool completed = false;

    public static event Action OnDownloadComplete;
    public static bool DownloadFinished = false;

    void OnEnable()
    {
        DownloadFinished = false;
        completed = false;
        currentProgress = 0f;

        if (downloadSlider)
            downloadSlider.value = 0f;
    }

    void Update()
    {
        if (completed) return;

        if (currentProgress >= 1f)
        {
            FinishDownload();
            return;
        }

        float currentSpeed = (currentProgress < 0.10f) ? firstHalfSpeed : secondHalfSpeed;
        float finalSpeed = currentSpeed + shakeBoost;

        currentProgress += finalSpeed * Time.deltaTime / 100f;

        if (downloadSlider) downloadSlider.value = currentProgress;
        if (speedText) speedText.text = finalSpeed.ToString("F2") + " kb/s";
    }

    void FinishDownload()
    {
        completed = true;
        DownloadFinished = true;
        currentProgress = 1f;

        if (downloadSlider) downloadSlider.value = 1f;
        if (speedText) speedText.text = "Download Complete";

        Debug.Log("🔥 DOWNLOAD COMPLETE");
        OnDownloadComplete?.Invoke();
    }
}