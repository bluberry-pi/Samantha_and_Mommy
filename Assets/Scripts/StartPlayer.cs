using UnityEngine;

public class StartPlayer : MonoBehaviour
{
    Animator anim;
    public GameObject Player;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("Start", true);
        }
    }
    public void DestroySelf()
    {
        Player.SetActive(true);
        Destroy(gameObject);
    }

}