using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource bgSource;
    public AudioSource momSource;

    public float bgDuckedVolume = 0.3f;   // BG volume while mom is active
    public float maxMomVolume = 1f;

    void Awake()
    {
        if (SceneBeginning.CutsceneActive) return;
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void SetMomThreat(float proximity01, bool momMoving)
    {
        if (!momMoving)
        {
            momSource.volume = 0;
            if (momSource.isPlaying) momSource.Stop();
            bgSource.volume = 1f;
            return;
        }

        // Start mom music if not playing
        if (!momSource.isPlaying)
            momSource.Play();

        // Duck background music
        bgSource.volume = bgDuckedVolume;

        // Linear volume increase based on distance
        momSource.volume = Mathf.Lerp(0f, maxMomVolume, proximity01);
    }
}