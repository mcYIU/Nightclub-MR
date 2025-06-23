using UnityEngine;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    public static void PlaySFXOnce(AudioClip clip)
    {
        Instance.audioSource.PlayOneShot(clip);
    }

    public static void PlaySFXLoop(AudioClip clip)
    {
        Instance.audioSource.loop = true;
        Instance.audioSource.clip = clip;
        Instance.audioSource.Play();
    }

    public static void StopSFXLoop()
    {
        Instance.audioSource.Stop();
        Instance.audioSource.loop = false;
    }
}
