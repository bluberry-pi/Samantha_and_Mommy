using UnityEngine;

public class EyesScript : MonoBehaviour
{
    public GameObject leftEye;
    public GameObject rightEye;

    public GameObject blockWhileExists;   // ← drag your cutscene controller here

    bool leftClosed = false;
    bool rightClosed = false;

    void Start()
    {
        leftEye.SetActive(false);
        rightEye.SetActive(false);
    }

    void Update()
    {
        if (blockWhileExists != null)
            return;

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