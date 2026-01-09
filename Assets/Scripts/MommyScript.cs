using System.Collections;
using UnityEngine;

public class MommyScript : MonoBehaviour
{
    public Rigidbody2D mommy;
    public float speed = 5f;
    public float rushSpeed = 10f;
    public float intimidateTime = 3f;

    public MomSlider momSlider;
    public EyesScript eyes;
    public SleepAnimTrig sleep;

    Vector2 startPos;
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
        if (momSlider.momAngry && !attacking && !returning && !hardWaiting)
            StartCoroutine(Attack());
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
        StopAllCoroutines();
        StartCoroutine(Rush());
    }

    IEnumerator Rush()
    {
        attacking = false;
        hardWaiting = true;

        mommy.linearVelocity = dir * rushSpeed;
        yield return new WaitForSeconds(intimidateTime);

        mommy.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(3f);

        hardWaiting = false;
        if (sleep.sleeping && eyes.leftEye.activeSelf && eyes.rightEye.activeSelf)
            StartCoroutine(Return());
        else
            StartCoroutine(Attack()); 
    }

    IEnumerator Return()
    {
        returning = true;
        momSlider.momAngry = false;

        while (Vector2.Distance(mommy.position, startPos) > 0.05f)
        {
            mommy.linearVelocity = (startPos - mommy.position).normalized * speed;
            yield return null;
        }

        mommy.position = startPos;
        mommy.linearVelocity = Vector2.zero;
        returning = false;
    }
}