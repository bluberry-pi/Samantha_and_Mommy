using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    public AudioClip bg_music;

    private void Start()
    {
        musicSource.clip = bg_music;
        musicSource.Play();
    }
}
