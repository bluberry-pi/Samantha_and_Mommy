using UnityEngine;
using UnityEngine.Rendering;

public class SoundFXManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static SoundFXManager instance;
    [SerializeField] private AudioSource soundFXObject;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        //spawn in gameObject;
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity);

        //assign the audioClip
        audioSource.volume = volume;

        //play sound
        audioSource.Play();

        //get length of the sound FX clip
        float clipLength = audioSource.clip.length;

        //destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);
        
    }
}
