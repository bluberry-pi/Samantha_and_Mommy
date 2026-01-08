using UnityEngine;

public class TurnPcOn : MonoBehaviour
{
    public GameObject OnButton;
    public GameObject PcLight;
    public GameObject PcScreen;
    public static bool pcPowered = false;


    bool enteredOnce = false;
    bool turnedPc = false;

    void Start()
    {
        PcLight.SetActive(false);
        PcScreen.SetActive(false);
        OnButton.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        if (TeddyPickup.teddyOnPc)
        {
            OnButton.SetActive(false);
            PcScreen.SetActive(false);
            return;
        }

        if (turnedPc)
            PcScreen.SetActive(true);
        else
            OnButton.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        enteredOnce = true;
        OnButton.SetActive(false);
        PcScreen.SetActive(false);
    }

    public void OnButtonPress()
    {
        if (TeddyPickup.teddyOnPc) return;

        PcLight.SetActive(true);
        PcScreen.SetActive(true);
        pcPowered = true;
        turnedPc = true;
    }
}