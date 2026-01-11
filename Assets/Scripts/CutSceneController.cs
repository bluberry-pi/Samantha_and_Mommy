using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public int nextSceneIndex = 2;
    bool loading = false;
    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        LoadNext();
    }
    public void SkipCutscene()
    {
        LoadNext();
    }
    
    void LoadNext()
    {
        if (loading) return;
        loading = true;

        SceneManager.LoadScene(nextSceneIndex);
    }
}