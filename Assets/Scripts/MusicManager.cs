using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
    }

    public static void PlayMusic(AudioClip clip)
    {
        Instance.audioSource.clip = clip;
        Instance.audioSource.Play();
    }

    public static void StopMusic()
    {
        Instance.audioSource.Stop();
    }
}
