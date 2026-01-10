using UnityEngine;
using TMPro;

public class PasswordChecker : MonoBehaviour
{
    public TMP_InputField inputField;

    [Header("Correct Password")]
    public string correctPassword = "hello123";

    public void CheckPassword()
    {
        string typed = inputField.text;

        if (typed == correctPassword)
        {
            Debug.Log("ACCESS GRANTED");

            // Destroy EVERYTHING tagged GameStuff
            GameObject[] stuff = GameObject.FindGameObjectsWithTag("GameStuff");

            foreach (GameObject g in stuff)
            {
                Destroy(g);
            }
        }
        else
        {
            Debug.Log("ACCESS DENIED");
        }

        inputField.text = "";
    }
}