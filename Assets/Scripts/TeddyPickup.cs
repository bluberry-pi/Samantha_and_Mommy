using UnityEngine;

public class TeddyPickup : MonoBehaviour
{
    public SpriteRenderer Teddy;
    public GameObject TeddyOnHand;
    public GameObject TeddyOnPc;
    public GameObject pcLight;

    public Collider2D bedZone;
    public Collider2D desktopZone;
    public static bool teddyOnPc = false;
    public GameObject eToPickup;

    bool playerNearBed;
    bool playerNearDesktop;
    bool holdingTeddy;

    Collider2D playerCol;

    void Start()
    {
        playerCol = GetComponent<Collider2D>();
    }

    void Update()
    {
        pcLight.SetActive(TurnPcOn.pcPowered && !teddyOnPc);

        playerNearBed = bedZone.IsTouching(playerCol);
        playerNearDesktop = desktopZone.IsTouching(playerCol);

        if (playerNearBed)
        {
            eToPickup.SetActive(true);
        } else {
            eToPickup.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            // PICK TEDDY FROM BED
            if (!holdingTeddy && playerNearBed && Teddy.enabled)
            {
                Debug.Log("ACTION: Pick Teddy From Bed");
                Teddy.enabled = false;
                TeddyOnHand.SetActive(true);
                TeddyOnPc.SetActive(false);
                holdingTeddy = true;
            }

            //DROP TEDDY ON BED
            else if (holdingTeddy && playerNearBed && !Teddy.enabled)
            {
                Debug.Log("ACTION: Drop Teddy On Bed");
                Teddy.enabled = true;
                TeddyOnHand.SetActive(false);
                TeddyOnPc.SetActive(false);
                holdingTeddy = false;
            }

            //PUT TEDDY ON PC
            else if (holdingTeddy && playerNearDesktop && !TeddyOnPc.activeSelf)
            {
                Debug.Log("ACTION: Put Teddy On PC");
                Teddy.enabled = false;
                TeddyOnHand.SetActive(false);
                TeddyOnPc.SetActive(true);
                holdingTeddy = false;
                teddyOnPc = true;
            }

            //TAKE TEDDY FROM PC
            else if (!holdingTeddy && playerNearDesktop && TeddyOnPc.activeSelf)
            {
                Debug.Log("ACTION: Take Teddy From PC");
                Teddy.enabled = false;
                TeddyOnHand.SetActive(true);
                TeddyOnPc.SetActive(false);
                holdingTeddy = true;
                teddyOnPc = false;
            }
        }
    }
}