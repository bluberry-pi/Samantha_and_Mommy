using UnityEngine;
using UnityEngine.UI;

public class EyesScript : MonoBehaviour
{
    public Image leftEye;
    public Image rightEye;

    public GameObject blockWhileExists;
    public GameObject peekObject;

    DownloadManager downloadManager;

    bool leftClosed = false;
    bool rightClosed = false;
    bool disableEyes = false; // FIX: Flag to disable eyes during game over

    public bool AreEyesClosed()   // 🔥 clean public API
    {
        return leftClosed && rightClosed;
    }

    void Start()
    {
        leftEye.gameObject.SetActive(false);
        rightEye.gameObject.SetActive(false);
        disableEyes = false;
    }

    void Update()
    {
        if (!downloadManager)
        {
            GameObject win = GameObject.FindGameObjectWithTag("UpdateWIndow");
            if (win)
                downloadManager = win.GetComponentInChildren<DownloadManager>();
        }

        if (blockWhileExists && blockWhileExists.activeInHierarchy)
            return;

        // FIX: Prevent eye input when disabled (during game over)
        if (disableEyes)
            return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            leftClosed = !leftClosed;
            leftEye.gameObject.SetActive(leftClosed);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            rightClosed = !rightClosed;
            rightEye.gameObject.SetActive(rightClosed);
        }

        Peek();
        UpdateEyeBonus();
    }

    void Peek()
    {
        if (!AreEyesClosed())
        {
            SetEyeOpacity(1f);
            if (peekObject) peekObject.SetActive(false);
            return;
        }

        if (peekObject) peekObject.SetActive(true);

        if (Input.GetKey(KeyCode.P))
            SetEyeOpacity(0.7f);
        else
            SetEyeOpacity(1f);
    }

    void UpdateEyeBonus()
    {
        if (!downloadManager) return;

        // FIX: Don't apply eye bonus when disabled
        if (disableEyes)
        {
            downloadManager.eyeBonus = 0f;
            return;
        }

        downloadManager.eyeBonus = AreEyesClosed() ? downloadManager.eyeClosedBonus : 0f;
    }

    void SetEyeOpacity(float alpha)
    {
        if (leftEye)
        {
            Color c = leftEye.color;
            c.a = alpha;
            leftEye.color = c;
        }

        if (rightEye)
        {
            Color c = rightEye.color;
            c.a = alpha;
            rightEye.color = c;
        }
    }

    // FIX: Force eyes open and disable functionality for game over
    public void ForceEyesOpen()
    {
        disableEyes = true;
        leftClosed = false;
        rightClosed = false;
        
        if (leftEye) leftEye.gameObject.SetActive(false);
        if (rightEye) rightEye.gameObject.SetActive(false);
        if (peekObject) peekObject.SetActive(false);
        
        // Remove eye bonus
        if (downloadManager)
            downloadManager.eyeBonus = 0f;
    }
}