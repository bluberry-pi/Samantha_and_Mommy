using UnityEngine;

public class EyesScript : MonoBehaviour
{
    public GameObject leftEye;
    public GameObject rightEye;

    bool leftClosed = false;
    bool rightClosed = false;

    void Start()
    {
        leftEye.SetActive(false);
        rightEye.SetActive(false);
    }

    void Update()
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