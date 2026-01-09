using UnityEngine;

public class ObstacleSpawn : MonoBehaviour
{
    public GameObject Obstacle;
    public float spawnRate = 2f;
    private float timer = 0f;
    public float widthOffset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawnObstacle();
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= spawnRate)
        {
            timer = timer + Time.deltaTime;
        } else
        {
             spawnObstacle();
             timer = 0;
        }
    }

    void spawnObstacle()
    {
        float lowestPoint = transform.position.x - widthOffset;
        float highestPoint = transform.position.x + widthOffset;
        Instantiate(Obstacle, new Vector3(Random.Range(lowestPoint, highestPoint), transform.position.y, 0), transform.rotation);
    }
}
