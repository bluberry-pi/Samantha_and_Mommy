using UnityEngine;
using UnityEngine.UI;

public class MomSlider : MonoBehaviour
{
    public Slider momSlider;
    public PlayerMovement playerMovement;

    public float increaseRate = 0.5f;
    public float decreaseRate = 1f;
    public float maxSliderValue = 100f;

    public Transform shakyWindow;
    public string shakyWindowTag = "UpdateWindow";
    public float minShakeIntensity = 0.06f;
    public float shakeIncreaseRate = 5f;
    public float shakeDecayRate = 0.2f;

    public AudioClip dragUpClip;
    public AudioClip dragDownClip;
    public float soundCooldown = 0.08f;

    public MomRandom momRandom;
    public bool momAngry = false;

    float current;
    float lastY;
    float lastVel;
    float soundTimer;
    bool shaking;

    void Start()
    {
        momSlider.maxValue = maxSliderValue;
        momSlider.value = 0f;
        FindWindow();
        if (shakyWindow)
            lastY = shakyWindow.position.y;
    }

    void Update()
    {
        if (SceneBeginning.CutsceneActive) return;
        FindWindow();

        if (momRandom && momRandom.CurrentState == MomRandom.MomState.Filling)
        {
            current += momRandom.randomFillSpeed * Time.deltaTime;
            if (current >= maxSliderValue)
            {
                current = maxSliderValue;
                momRandom.NotifyReachedMax();
            }
        }
        else if (momRandom && momRandom.CurrentState == MomRandom.MomState.Draining)
        {
            current -= momRandom.randomDrainSpeed * Time.deltaTime;
            if (current <= 0f)
            {
                current = 0f;
                momRandom.NotifyReachedZero();
            }
        }
        else
        {
            HandleShake();
            HandleWalk();
        }

        current = Mathf.Clamp(current, 0f, maxSliderValue);
        momSlider.value = current;
        momAngry = current >= maxSliderValue;
    }

    void HandleWalk()
    {
        // FIX: Check if PlayerMovement is enabled before checking movement
        // This prevents the slider from increasing when player is asleep
        if (playerMovement != null && 
            playerMovement.enabled && 
            playerMovement.GetMovement().magnitude > 0.01f)
        {
            current += increaseRate * Time.deltaTime;
        }
        else if (!shaking)
        {
            current -= decreaseRate * Time.deltaTime;
        }
        else
        {
            current -= shakeDecayRate * Time.deltaTime;
        }
    }

    void HandleShake()
    {
        if (!shakyWindow) return;

        float y = shakyWindow.position.y;
        float vel = y - lastY;
        float accel = Mathf.Abs(vel - lastVel);

        lastY = y;
        lastVel = vel;

        if (accel > minShakeIntensity)
        {
            current += accel * shakeIncreaseRate;
            shaking = true;

            soundTimer -= Time.deltaTime;
            float soundPower = accel * 2.5f;

            if (soundPower > 0.02f && soundTimer <= 0f)
            {
                AudioClip clip = vel > 0 ? dragUpClip : dragDownClip;
                float volume = Mathf.Clamp01(Mathf.Pow(soundPower, 0.4f));
                float pitch = Mathf.Lerp(0.9f, 1.6f, Mathf.Clamp01(soundPower));
                SoundFXManager.instance.PlaySoundFXClip(clip, shakyWindow, volume, pitch);
                soundTimer = Mathf.Lerp(0.09f, 0.01f, Mathf.Clamp01(soundPower * 1.5f));
            }
        }
        else
        {
            shaking = false;
        }
    }

    void FindWindow()
    {
        if (shakyWindow) return;

        GameObject w = GameObject.FindGameObjectWithTag(shakyWindowTag);
        if (w)
        {
            shakyWindow = w.transform;
            lastY = shakyWindow.position.y;
        }
    }

    // FIX: Added method to reset the slider value
    public void ResetSlider()
    {
        current = 0f;
        momSlider.value = 0f;
    }
}