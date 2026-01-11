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
        ResetCalm();
    }

    void Update()
    {
        if (SceneBeginning.CutsceneActive) return;
        if (!enabledRandom) return;

        timer -= Time.deltaTime;

        if (timer > 0f) return;

        if (CurrentState == MomState.Calm)
            CurrentState = MomState.Filling;
        else if (CurrentState == MomState.Cooldown)
            ResetCalm();
    }

    public void NotifyReachedMax()
    {
        if (CurrentState == MomState.Filling)
            CurrentState = MomState.Draining;
    }

    public void NotifyReachedZero()
    {
        if (CurrentState == MomState.Draining)
        {
            CurrentState = MomState.Cooldown;
            timer = cooldownAfterDrain;
        }
    }

    void ResetCalm()
    {
        CurrentState = MomState.Calm;
        timer = Random.Range(minCalmTime, maxCalmTime);
    }
}