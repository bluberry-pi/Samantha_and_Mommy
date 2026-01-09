using UnityEngine;
using System.Collections;

public class SleepAnimTrig : MonoBehaviour
{
    PlayerMovement movementScript;
    public GameObject sleepAnimation;
    public GameObject wakeupAnimation;
    public GameObject player;
    public GameObject fToInteract;

    bool nearBed = false;
    bool sleeping = false;
    bool busy = false;

    GameObject sleepAnim;
    GameObject wakeAnim;

    SpriteRenderer playerSprite;
    Collider2D playerCollider;

    void Start()
    {
        playerSprite = player.GetComponent<SpriteRenderer>();
        playerCollider = player.GetComponent<Collider2D>();
        movementScript = player.GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (busy) return;

            if (!sleeping && nearBed)
            {
                StartCoroutine(Sleep());
            }
            else if (sleeping)
            {
                StartCoroutine(WakeUp());
            }
        }
    }

    IEnumerator Sleep()
    {
        busy = true;

        playerSprite.enabled = false;
        playerCollider.enabled = false;
        movementScript.enabled = false;

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
        movementScript.enabled = true;

        sleeping = false;
        busy = false;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            nearBed = true;
            fToInteract.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            nearBed = false;
            fToInteract.SetActive(false);
        }
    }
}