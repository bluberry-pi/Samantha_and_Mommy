using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    public float moveSpeed = 7f;
    void Update()
    {
        transform.position = transform.position + (Vector3.down * moveSpeed) * Time.deltaTime;
    }
}
