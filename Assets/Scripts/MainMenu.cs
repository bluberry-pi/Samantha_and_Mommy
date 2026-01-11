using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuPlay : MonoBehaviour
{
    public void OnPlayPress()
    {
        SceneManager.LoadScene(1);
    }
}