using UnityEngine;

public class MomRandom : MonoBehaviour
{
    public bool enabledRandom = true;
    public float minCalmTime = 6f;
    public float maxCalmTime = 14f;
    public float cooldownAfterDrain = 10f;
    public float randomFillSpeed = 2f;
    public float randomDrainSpeed = 1f;

    public enum MomState { Calm, Filling, Draining, Cooldown }
    public MomState CurrentState { get; private set; } = MomState.Calm;

    float timer;

    void Start()
    {
        Debug.Log("[MomRandom] START - Initializing");
        ResetCalm();
    }

    void Update()
    {
        if (SceneBeginning.CutsceneActive)
        {
            Debug.Log("[MomRandom] Update - Cutscene active, returning");
            return;
        }
        
        if (!enabledRandom)
        {
            Debug.Log("[MomRandom] Update - Random disabled, returning");
            return;
        }

        timer -= Time.deltaTime;

        if (timer > 0f) return;

        if (CurrentState == MomState.Calm)
        {
            Debug.Log("[MomRandom] State Change: CALM → FILLING");
            CurrentState = MomState.Filling;
        }
        else if (CurrentState == MomState.Cooldown)
        {
            Debug.Log("[MomRandom] State Change: COOLDOWN → Resetting to CALM");
            ResetCalm();
        }
    }

    public void NotifyReachedMax()
    {
        Debug.Log($"[MomRandom] NotifyReachedMax called - Current State: {CurrentState}");
        if (CurrentState == MomState.Filling)
        {
            Debug.Log("[MomRandom] State Change: FILLING → DRAINING");
            CurrentState = MomState.Draining;
        }
        else
        {
            Debug.LogWarning($"[MomRandom] NotifyReachedMax ignored - State is {CurrentState}, not Filling");
        }
    }

    public void NotifyReachedZero()
    {
        Debug.Log($"[MomRandom] NotifyReachedZero called - Current State: {CurrentState}");
        if (CurrentState == MomState.Draining)
        {
            Debug.Log($"[MomRandom] State Change: DRAINING → COOLDOWN (timer set to {cooldownAfterDrain}s)");
            CurrentState = MomState.Cooldown;
            timer = cooldownAfterDrain;
        }
        else
        {
            Debug.LogWarning($"[MomRandom] NotifyReachedZero ignored - State is {CurrentState}, not Draining");
        }
    }

    void ResetCalm()
    {
        Debug.Log("[MomRandom] ResetCalm called");
        CurrentState = MomState.Calm;
        timer = Random.Range(minCalmTime, maxCalmTime);
        Debug.Log($"[MomRandom] State set to CALM, timer set to {timer}s");
    }

    // FIX: Added method to force reset state when mom attacks and returns
    public void ForceReset()
    {
        Debug.Log($"[MomRandom] ⚠️ FORCE RESET CALLED - Previous State: {CurrentState}");
        CurrentState = MomState.Cooldown;
        timer = cooldownAfterDrain;
        Debug.Log($"[MomRandom] ✅ State forced to COOLDOWN, timer set to {cooldownAfterDrain}s");
    }

    // Debug helper to check state
    void LateUpdate()
    {
        if (CurrentState == MomState.Filling)
        {
            Debug.Log($"[MomRandom] 🔴 Currently FILLING - Speed: {randomFillSpeed}/s");
        }
    }
}