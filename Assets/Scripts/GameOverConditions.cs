using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOverConditions : MonoBehaviour
{
    public SleepAnimTrig sleep;
    public EyesScript eyes;
    public GameObject momPart1;
    public GameObject momPart2;
    public GameObject GameOverScreen;
    Coroutine flickerRoutine;
    float flickerSpeed = 0.15f;
    [SerializeField] AudioClip whyPcOnPart1;
    [SerializeField] AudioClip whyPcOnPart2;
    [SerializeField] AudioClip whyEyesOpenPart1;
    [SerializeField] AudioClip whyEyesOpenPart2;
    [SerializeField] AudioClip whyNoSleep;
    bool gameOver = false;

    public bool PlayerIsSafe()
    {
        if (TurnPcOn.PcIsOn)
        {
            // FIX: Force eyes open BEFORE checking anything
            if (eyes) eyes.ForceEyesOpen();
            StartCoroutine(PcOnSequence());
            return false;
        }

        if (sleep.sleeping && !eyes.AreEyesClosed())
        {
            Debug.Log("why are u awake samantha");
            // FIX: Force eyes open BEFORE checking anything
            if (eyes) eyes.ForceEyesOpen();
            StartCoroutine(WhyEyesOpenSequence());
            return false;
        }

        if (!sleep.sleeping)
        {
            // FIX: Force eyes open BEFORE checking anything
            if (eyes) eyes.ForceEyesOpen();
            StartCoroutine(NoSleepSequence());
            return false;
        }

        return true;
    }

    bool pcSequenceRunning = false;
    bool eyeSequenceRunning = false;
    bool noSleepRunning = false;

    IEnumerator PcOnSequence()
    {
        if (pcSequenceRunning) yield break;
        pcSequenceRunning = true;

        yield return SoundFXManager.instance.PlayAndWait(whyPcOnPart1, transform, 1f);

        momPart1.SetActive(false);
        momPart2.SetActive(true);

        yield return SoundFXManager.instance.PlayAndWait(whyPcOnPart2, transform, 1f);

        momPart2.SetActive(false);
        momPart1.SetActive(true);

        pcSequenceRunning = false;

        TriggerGameOver();
    }


    IEnumerator WhyEyesOpenSequence()
    {
        if (eyeSequenceRunning) yield break;
        eyeSequenceRunning = true;

        yield return SoundFXManager.instance.PlayAndWait(whyEyesOpenPart1, transform, 1f);

        momPart1.SetActive(false);
        momPart2.SetActive(true);

        yield return SoundFXManager.instance.PlayAndWait(whyEyesOpenPart2, transform, 1f);

        momPart2.SetActive(false);
        momPart1.SetActive(true);

        eyeSequenceRunning = false;

        TriggerGameOver();
    }


    IEnumerator NoSleepSequence()
    {
        if (noSleepRunning) yield break;
        noSleepRunning = true;
        flickerRoutine = StartCoroutine(MomFlicker());

        yield return SoundFXManager.instance.PlayAndWait(whyNoSleep, transform, 1f);

        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        momPart2.SetActive(false);
        momPart1.SetActive(true);

        noSleepRunning = false;

        TriggerGameOver();
    }
    IEnumerator MomFlicker()
    {
        while (true)
        {
            momPart1.SetActive(false);
            momPart2.SetActive(true);
            yield return new WaitForSeconds(flickerSpeed);

            momPart2.SetActive(false);
            momPart1.SetActive(true);
            yield return new WaitForSeconds(flickerSpeed);
        }
    }
    public void TriggerGameOver()
    {
        if (gameOver) return;
        gameOver = true;

        StartCoroutine(DelayedGameOver());
    }
    IEnumerator DelayedGameOver()
    {
        yield return new WaitForSeconds(0.1f);

        Time.timeScale = 0f;

        if (GameOverScreen)
            GameOverScreen.SetActive(true);
    }

    public void ReloadScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}