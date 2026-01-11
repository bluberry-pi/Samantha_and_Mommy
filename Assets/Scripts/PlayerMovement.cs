using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D body;
    private Vector2 movement;
    public Animator animator;

    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (SceneBeginning.CutsceneActive) return;
        movement.x = Input.GetAxisRaw("WASD_Horizontal");
        movement.y = Input.GetAxisRaw("WASD_Vertical");
        movement.Normalize();

        if (movement.x > 0)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else if (movement.x < 0)
            transform.localScale = originalScale;

        if(movement.x != 0 || movement.y != 0)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }

    void FixedUpdate()
    {
        body.MovePosition(body.position + movement * speed * Time.fixedDeltaTime);
    }

    public Vector2 GetMovement()
    {
        return movement;
    }
}