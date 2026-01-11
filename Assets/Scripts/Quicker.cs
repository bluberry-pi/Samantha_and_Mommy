using UnityEngine;

public class Quicker : MonoBehaviour
{
    public GameObject instruction;
    public float lifeTime = 2f;   // how long before it dies

    float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifeTime)
        {
            Destroy(instruction);
        }
    }
}