using System.Collections;
using UnityEngine;

public class MommyScript : MonoBehaviour
{
    public Rigidbody2D mommy;
    public float speed = 5f;
    public float speedAfter = 10f;
    public float stopAfter = 1f;

    Vector2 direction;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Movement();
        }
    }

    void Movement()
    {
        direction = Vector2.right;
        mommy.linearVelocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        StopAllCoroutines();
        StartCoroutine(RushAndStop());
    }

    IEnumerator RushAndStop()
    {
        mommy.linearVelocity = direction * speedAfter;
        yield return new WaitForSeconds(stopAfter);
        mommy.linearVelocity = Vector2.zero;
    }
}
