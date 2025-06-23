using UnityEngine;

public class FaderController : MonoBehaviour
{
    public static FaderController Instance;
    [SerializeField] private OVRScreenFade[] faders;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void FadeIn() { foreach (var _f in Instance.faders) _f.FadeIn(); }

    public static void FadeOut() { foreach (var _f in Instance.faders) _f.FadeOut(); }

}
