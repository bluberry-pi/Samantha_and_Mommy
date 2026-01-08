using UnityEngine;

public class DownloadSpeedIncreaseInstances : MonoBehaviour
{
    public DownloadManager downloadManager;
    public WindowRotation windowRotation;

    [Header("Shake Settings")]
    public float minShakeSpeed = 2f;
    public float buildSpeed = 1.4f;
    public float decaySpeed = 0.8f;
    public float maxShakeBoost = 2f;

    [Header("Vertical Bonus")]
    public float verticalBaseBoost = 0.5f;   // Always active when vertical

    float lastY;
    float shakeBoost = 0f;

    void Start()
    {
        lastY = transform.position.y;
    }

    void Update()
    {
        float totalBoost = 0f;

        if (windowRotation.vertical)
        {
            totalBoost += GetVerticalBonus();
            totalBoost += GetShakeBonus();
        }
        else
        {
            // If not vertical, shake bonus decays
            shakeBoost = Mathf.Max(0f, shakeBoost - Time.deltaTime * decaySpeed);
        }

        downloadManager.shakeBoost = totalBoost;
    }
    float GetVerticalBonus()
    {
        return verticalBaseBoost;
    }
    float GetShakeBonus()
    {
        float currentY = transform.position.y;
        float ySpeed = Mathf.Abs(currentY - lastY) / Mathf.Max(Time.deltaTime, 0.0001f);
        lastY = currentY;

        if (ySpeed > minShakeSpeed)
        {
            shakeBoost += (ySpeed * 0.02f) * buildSpeed;
        }
        else
        {
            shakeBoost -= Time.deltaTime * decaySpeed;
        }

        shakeBoost = Mathf.Clamp(shakeBoost, 0f, maxShakeBoost);
        return shakeBoost;
    }
}