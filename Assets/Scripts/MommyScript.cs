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


    Vector2 startPos;
    Vector2 lastKnownPlayerPos;
    Vector2 dir = Vector2.right;

    bool attacking;
    bool returning;
    bool hardWaiting;

    void Start()
    {
        startPos = mommy.position;
    }

    void Update()
    {
        if (SceneBeginning.CutsceneActive) return;
        if (momSlider.momAngry && !attacking && !returning && !hardWaiting)
            StartCoroutine(Attack());

        UpdateThreatMusic();
    }


    IEnumerator Attack()
    {
        attacking = true;
        mommy.linearVelocity = dir * speed;
        yield return null;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!attacking) return;
        //StopAllCoroutines();
        StartCoroutine(Rush());
    }

    IEnumerator Rush()
    {
        attacking = false;
        hardWaiting = true;

        mommy.linearVelocity = dir * rushSpeed;
        yield return new WaitForSeconds(intimidateTime);

        mommy.linearVelocity = Vector2.zero;
        if (brain.PlayerIsSafe())
        {
            yield return new WaitForSeconds(waitTime);
            hardWaiting = false;
            StartCoroutine(Return());
        }
        else
        {
            Debug.Log("GAME OVER");
        }
    }

    IEnumerator Return()
    {
        returning = true;
        momSlider.momAngry = false;

        while (Vector2.Distance(mommy.position, startPos) > 0.05f)
        {
            mommy.linearVelocity = (startPos - mommy.position).normalized * returnSpeed;
            yield return null;
        }

        mommy.position = startPos;
        mommy.linearVelocity = Vector2.zero;
        returning = false;
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