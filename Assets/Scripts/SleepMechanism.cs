using UnityEngine;
using System.Collections;

public class SleepAnimTrig : MonoBehaviour
{
    public GameObject sleepAnimation;
    public GameObject wakeupAnimation;
    public GameObject player;
    public GameObject leftEye;
    public GameObject rightEye;
    public GameObject fToInteract;

    bool nearBed = false;
    bool sleeping = false;
    bool busy = false;

    bool leftClosed = false;
    bool rightClosed = false;

    GameObject sleepAnim;
    GameObject wakeAnim;

    SpriteRenderer playerSprite;
    Collider2D playerCollider;

    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        playerCollider = player.GetComponent<Collider2D>();

        leftEye.SetActive(false);
        rightEye.SetActive(false);
    }

    void Update()
    {
        // SLEEP / WAKE KEY
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (busy) return;

            if (!sleeping && nearBed)
            {
                StartCoroutine(Sleep());
            }
            else if (sleeping)
            {
                // BLOCK wake if BOTH eyes are closed
                if (leftClosed && rightClosed)
                {
                    Debug.Log("Can't wake up — BOTH eyes are closed!");
                    return;
                }

                StartCoroutine(WakeUp());
            }
        }

        if (sleeping && !busy)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                leftClosed = !leftClosed;
                leftEye.SetActive(leftClosed);
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                rightClosed = !rightClosed;
                rightEye.SetActive(rightClosed);
            }
        }

    }

    IEnumerator Sleep()
    {
        busy = true;

        playerSprite.enabled = false;
        playerCollider.enabled = false;

        sleepAnim = Instantiate(sleepAnimation);
        sleeping = true;

        yield return new WaitForSeconds(0.1f);

        busy = false;
    }

    IEnumerator WakeUp()
    {
        busy = true;

        if (sleepAnim) Destroy(sleepAnim);

        wakeAnim = Instantiate(wakeupAnimation);
        Animator anim = wakeAnim.GetComponent<Animator>();

        yield return null;
        float length = anim.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(length);

        Destroy(wakeAnim);

        playerSprite.enabled = true;
        playerCollider.enabled = true;

        // Reset eyes
        leftClosed = false;
        rightClosed = false;
        leftEye.SetActive(false);
        rightEye.SetActive(false);

        sleeping = false;
        busy = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            nearBed = true;
        fToInteract.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            nearBed = false;
        fToInteract.SetActive(false);
    }
}