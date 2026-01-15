using System.Collections;
using UnityEngine;

public class MommyScript : MonoBehaviour
{
    [Header("Threat Audio Distances")]
    public float audibleStartDistance = 20f;
    public float fullVolumeDistance = 6f;

    [Header("Threat Audio")]
    public Transform player;
    //public float maxThreatDistance = 10f;

    public Rigidbody2D mommy;
    public float speed = 5f;
    public float rushSpeed = 10f;
    public float intimidateTime = 3f;
    public float waitTime = 2f;

    public MomSlider momSlider;
    public EyesScript eyes;
    public SleepAnimTrig sleep;
    public TurnPcOn pcLight;
    public GameOverConditions brain;
    public float returnSpeed = 7f;

    // FIX: Added reference to MomRandom to reset its state
    public MomRandom momRandom;

    Vector2 startPos;
    Vector2 lastKnownPlayerPos;
    Vector2 dir = Vector2.right;

    bool attacking;
    bool returning;
    bool hardWaiting;

    void Start()
    {
        Debug.Log("[MommyScript] START - Initializing");
        startPos = mommy.position;
        Debug.Log($"[MommyScript] Start position: {startPos}");
        
        // FIX: Auto-find MomRandom if not assigned
        if (momRandom == null && momSlider != null)
        {
            Debug.Log("[MommyScript] MomRandom not assigned, trying to auto-find from MomSlider");
            momRandom = momSlider.momRandom;
            if (momRandom != null)
            {
                Debug.Log($"[MommyScript] ✅ Auto-found MomRandom: {momRandom.gameObject.name}");
            }
            else
            {
                Debug.LogError("[MommyScript] ❌ FAILED to auto-find MomRandom! PLEASE ASSIGN IN INSPECTOR!");
            }
        }
        else if (momRandom != null)
        {
            Debug.Log($"[MommyScript] ✅ MomRandom assigned: {momRandom.gameObject.name}");
        }
        else
        {
            Debug.LogError("[MommyScript] ❌ MomRandom is NULL and couldn't be found! PLEASE ASSIGN IN INSPECTOR!");
        }
    }

    void Update()
    {
        if (SceneBeginning.CutsceneActive) return;
        
        // Log when conditions are checked
        if (momSlider.momAngry && !attacking && !returning && !hardWaiting)
        {
            Debug.Log("[MommyScript] 🚨 MOM IS ANGRY - Starting Attack!");
            Debug.Log($"[MommyScript] States - Attacking: {attacking}, Returning: {returning}, HardWaiting: {hardWaiting}");
            StartCoroutine(Attack());
        }

        UpdateThreatMusic();
    }


    IEnumerator Attack()
    {
        Debug.Log("[MommyScript] ⚔️ ATTACK COROUTINE STARTED");
        attacking = true;
        mommy.linearVelocity = dir * speed;
        Debug.Log($"[MommyScript] Moving at speed: {speed}");
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!attacking)
        {
            Debug.Log($"[MommyScript] Trigger entered but not attacking: {col.gameObject.name}");
            return;
        }
        
        Debug.Log($"[MommyScript] 💥 TRIGGER ENTERED WHILE ATTACKING: {col.gameObject.name}");
        //StopAllCoroutines();
        StartCoroutine(Rush());
    }

    IEnumerator Rush()
    {
        Debug.Log("[MommyScript] 🏃 RUSH COROUTINE STARTED");
        attacking = false;
        hardWaiting = true;

        mommy.linearVelocity = dir * rushSpeed;
        Debug.Log($"[MommyScript] Rushing at speed: {rushSpeed} for {intimidateTime}s");
        yield return new WaitForSeconds(intimidateTime);

        mommy.linearVelocity = Vector2.zero;
        Debug.Log("[MommyScript] Checking if player is safe...");
        
        if (brain.PlayerIsSafe())
        {
            Debug.Log($"[MommyScript] ✅ Player is SAFE - Waiting {waitTime}s before returning");
            yield return new WaitForSeconds(waitTime);
            hardWaiting = false;
            Debug.Log("[MommyScript] Starting Return coroutine");
            StartCoroutine(Return());
        }
        else
        {
            Debug.Log("[MommyScript] ☠️ GAME OVER - Player is NOT safe");
        }
    }

    IEnumerator Return()
    {
        Debug.Log("[MommyScript] 🔙 RETURN COROUTINE STARTED");
        Debug.Log($"[MommyScript] Current position: {mommy.position}, Start position: {startPos}");
        
        returning = true;
        momSlider.momAngry = false;
        Debug.Log("[MommyScript] Set momAngry to false");
        
        momSlider.ResetSlider();
        Debug.Log("[MommyScript] Called ResetSlider()");
        
        // FIX: Reset MomRandom state to prevent infinite loop
        if (momRandom != null)
        {
            Debug.Log($"[MommyScript] 🔄 Calling MomRandom.ForceReset() - Current state: {momRandom.CurrentState}");
            momRandom.ForceReset();
            Debug.Log($"[MommyScript] ✅ MomRandom.ForceReset() completed - New state: {momRandom.CurrentState}");
        }
        else
        {
            Debug.LogError("[MommyScript] ❌ CRITICAL: MomRandom is NULL! Cannot reset state! LOOP WILL CONTINUE!");
        }

        float distanceToStart = Vector2.Distance(mommy.position, startPos);
        Debug.Log($"[MommyScript] Starting return journey - Distance: {distanceToStart}");
        
        while (Vector2.Distance(mommy.position, startPos) > 0.05f)
        {
            mommy.linearVelocity = (startPos - mommy.position).normalized * returnSpeed;
            yield return null;
        }

        mommy.position = startPos;
        mommy.linearVelocity = Vector2.zero;
        Debug.Log("[MommyScript] ✅ Reached start position");
        
        returning = false;
        Debug.Log("[MommyScript] 🏁 RETURN COMPLETE - All states reset");
        Debug.Log($"[MommyScript] Final check - MomRandom state: {(momRandom != null ? momRandom.CurrentState.ToString() : "NULL")}");
    }
    
    void UpdateThreatMusic()
    {
        if (player && player.gameObject.activeInHierarchy)
            lastKnownPlayerPos = player.position;

        bool momMoving = mommy.linearVelocity.magnitude > 0.05f;

        if (!momMoving)
        {
            AudioManager.Instance.SetMomThreat(0f, false);
            return;
        }

        float dist = Vector2.Distance(mommy.position, lastKnownPlayerPos);

        float raw = Mathf.InverseLerp(audibleStartDistance, fullVolumeDistance, dist);

        float lowCurve = Mathf.Pow(raw, 1.6f);
        float spike = Mathf.Pow(raw, 5.0f);
        float proximity01 = Mathf.Lerp(lowCurve, spike, raw);

        AudioManager.Instance.SetMomThreat(proximity01, true);
    }

}