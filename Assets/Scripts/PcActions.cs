using UnityEngine;
using System.Collections;

public class PcActions : MonoBehaviour
{
    public GameObject loadingScreen;
    public GameObject updateScreen;
    public float waitAfterLoad = 3f;
    public GameObject proceedButton;
    public GameObject Updating;
    public GameObject paintArea;
    public Transform paintParent;

    public Transform uiParent;

    GameObject updateInstance;
    GameObject loadingInstance;
    GameObject paintWindow;

    bool cancelled = false;

    void Update()
    {
        // If loading screen disappears → kill everything
        if (!cancelled && loadingInstance == null)
        {
            CancelEverything();
        }

        if (!updateInstance) return;

        float z = updateInstance.transform.eulerAngles.z;

        if (IsNear(z, 0f))
        {
            Debug.Log("0 degrees");
        }
        else if (IsNear(z, 270f))
        {
            Debug.Log("-90 degrees");
        }
        else if (IsNear(z, 180f))
        {
            Debug.Log("-180 degrees");
        }
        else if (IsNear(z, 90f))
        {
            Debug.Log("-270 degrees");
        }
    }

    public void OnGamePress()
    {
        cancelled = false;
        StartCoroutine(LoadSequence());
    }

    public void onPaintPress()
    {
        paintWindow = Instantiate(paintArea, paintParent);
    }

    IEnumerator LoadSequence()
    {
        loadingInstance = Instantiate(loadingScreen, uiParent);

        yield return new WaitForSeconds(waitAfterLoad);

        if (cancelled) yield break;

        updateInstance = Instantiate(updateScreen, uiParent);
        proceedButton.SetActive(true);
    }

    public void OnProceedPress()
    {
        if (cancelled) return;

        proceedButton.SetActive(false);
        Instantiate(Updating, uiParent);
        Destroy(updateInstance);
    }

    void CancelEverything()
    {
        cancelled = true;
        StopAllCoroutines();

        if (updateInstance) Destroy(updateInstance);
        if (loadingInstance) Destroy(loadingInstance);

        proceedButton.SetActive(false);
    }

    bool IsNear(float a, float b)
    {
        return Mathf.Abs(Mathf.DeltaAngle(a, b)) < 2f;
    }
}