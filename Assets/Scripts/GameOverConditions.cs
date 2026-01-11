using UnityEngine;
using System.Collections;

public class GameOverConditions : MonoBehaviour
{
    public SleepAnimTrig sleep;
    public EyesScript eyes;
    public GameObject momPart1;
    public GameObject momPart2;
    Coroutine flickerRoutine;
    float flickerSpeed = 0.15f;
    [SerializeField] AudioClip whyPcOnPart1;
    [SerializeField] AudioClip whyPcOnPart2;
    [SerializeField] AudioClip whyEyesOpenPart1;
    [SerializeField] AudioClip whyEyesOpenPart2;
    [SerializeField] AudioClip whyNoSleep;

    public bool PlayerIsSafe()
    {
        if (TurnPcOn.PcIsOn)
        {
            StartCoroutine(PcOnSequence());
            return false;
        }

        if (sleep.sleeping && (!eyes.leftEye.activeSelf || !eyes.rightEye.activeSelf))
        {
            Debug.Log("why are u awake samantha");
            StartCoroutine(WhyEyesOpenSequence());
            return false;
        }

        if (!sleep.sleeping)
        {
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

        // PART 1
        yield return SoundFXManager.instance.PlayAndWait(whyPcOnPart1, transform, 1f);

        momPart1.SetActive(false);
        momPart2.SetActive(true);

        // PART 2
        yield return SoundFXManager.instance.PlayAndWait(whyPcOnPart2, transform, 1f);

        momPart2.SetActive(false);
        momPart1.SetActive(true);

        pcSequenceRunning = false;
    }

    IEnumerator WhyEyesOpenSequence()
    {
        if (eyeSequenceRunning) yield break;
        eyeSequenceRunning = true;

        // PART 1
        yield return SoundFXManager.instance.PlayAndWait(whyEyesOpenPart1, transform, 1f);

        momPart1.SetActive(false);
        momPart2.SetActive(true);

        // PART 2
        yield return SoundFXManager.instance.PlayAndWait(whyEyesOpenPart2, transform, 1f);

        momPart2.SetActive(false);
        momPart1.SetActive(true);

        eyeSequenceRunning = false;
    }

    IEnumerator NoSleepSequence()
    {
        if (noSleepRunning) yield break;
        noSleepRunning = true;

        // Start flickering mom while audio plays
        flickerRoutine = StartCoroutine(MomFlicker());

        yield return SoundFXManager.instance.PlayAndWait(whyNoSleep, transform, 1f);

        // Stop flicker and restore mom
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        momPart2.SetActive(false);
        momPart1.SetActive(true);

        noSleepRunning = false;
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

}
