using UnityEngine;
using UnityEngine.UI;
public class MomSlider : MonoBehaviour
{
    public Slider momSlider;
    public PlayerMovement playerMovement;
    public WindowRotation windowRotation;
    [Header("Slider Settings")]
    public float increaseRate = 0.5f;
    public float decreaseRate = 1f;
    public float maxSliderValue = 100f;
    [Header("Shake Detection")]
    public Transform shakyWindow;
    public string shakyWindowTag = "UpdateWindow";
    public float minShakeIntensity = 0.5f;
    public float shakeIncreaseRate = 5f;
    public float shakeDecayRate = 0.2f;
    [Header("Random Mom Brain")]
    public MomRandom momRandom;
    public bool momAngry = false;
    float currentSliderValue;
    float lastY, lastVelocity;
    bool isCurrentlyShaking;
    
    void Start()
    {
        momSlider.maxValue = maxSliderValue;
        momSlider.value = 0f;
    }
    
    void Update()
    {
        FindWindowIfNeeded();
        HandleMomRandom();
        momSlider.value = currentSliderValue;
        
        if (momSlider.value >= maxSliderValue)
        {
            momAngry = true;
            Debug.Log("[MomSlider] Mom is coming...");
        }
    }
    
    void HandleMomRandom()
    {
        if (momRandom != null && momRandom.CurrentState == MomRandom.MomState.Filling)
        {
            currentSliderValue += momRandom.randomFillSpeed * Time.deltaTime;
            currentSliderValue = Mathf.Clamp(currentSliderValue, 0f, maxSliderValue);
            
            if (currentSliderValue >= maxSliderValue)
            {
                momRandom.NotifyReachedMax();
                Debug.Log("[MomSlider] Reached MAX, notified MomRandom");
            }
            return;
        }
        
        if (momRandom != null && momRandom.CurrentState == MomRandom.MomState.Draining)
        {
            currentSliderValue -= momRandom.randomDrainSpeed * Time.deltaTime;
            currentSliderValue = Mathf.Clamp(currentSliderValue, 0f, maxSliderValue);
            
            if (currentSliderValue <= 0f)
            {
                momRandom.NotifyReachedZero();
                Debug.Log("[MomSlider] Reached ZERO, notified MomRandom");
            }
            return;
        }
        
        HandleWalking();
        HandleShaking();
        currentSliderValue = Mathf.Clamp(currentSliderValue, 0f, maxSliderValue);
    }
    
    void HandleWalking()
    {
        bool moving = playerMovement.GetMovement().magnitude > 0.01f;
        if (moving)
            currentSliderValue += increaseRate * Time.deltaTime;
        else if (!isCurrentlyShaking)
            currentSliderValue -= decreaseRate * Time.deltaTime;
        else
            currentSliderValue -= shakeDecayRate * Time.deltaTime;
    }
    
    void HandleShaking()
    {
        if (windowRotation == null || !windowRotation.vertical || shakyWindow == null) return;
        float y = shakyWindow.position.y;
        float vel = (y - lastY) / Time.deltaTime;
        float delta = Mathf.Abs(vel - lastVelocity);
        lastY = y;
        lastVelocity = vel;
        if (delta > minShakeIntensity)
        {
            isCurrentlyShaking = true;
            currentSliderValue += delta * shakeIncreaseRate * Time.deltaTime;
        }
        else
        {
            isCurrentlyShaking = false;
        }
    }
    
    void FindWindowIfNeeded()
    {
        if (shakyWindow != null) return;
        GameObject w = GameObject.FindGameObjectWithTag(shakyWindowTag);
        if (w)
        {
            shakyWindow = w.transform;
            lastY = shakyWindow.position.y;
        }
    }
}