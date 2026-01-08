using UnityEngine;

public class SleepAnimTrig : MonoBehaviour
{
    public GameObject sleepAnimation;
    public GameObject player;

    bool nearBed = false;
    GameObject sleepAnim;

    void Update()
    {
        if (nearBed && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Sleep!");

            player.SetActive(false);
            sleepAnim = Instantiate(sleepAnimation);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bed"))
        {
            nearBed = true;
            Debug.Log("Entered Bed");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Bed"))
        {
            nearBed = false;
        }
    }
}