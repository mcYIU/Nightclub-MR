using System.Collections;
using UnityEngine;

public class Letter_Fire : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireVisual;
    [SerializeField] private float burningDuration;
    [SerializeField] private float burningDelay;
    [SerializeField] private string alphaClipPropertyName = "_Cutoff";
    [SerializeField] private float targetAlphaThreshold = 1.0f;
    [SerializeField] private AudioClip SFX;
    [SerializeField] private Interactable interactable;

    private Renderer objectRenderer;
    private float initialAlphaThreshold;
    private Color initialColor;
    private Color targetColor = Color.black;

    private bool isLighted = false;

    private void Start()
    {
        // Initializes the renderer and stores the initial color and alpha threshold of the material
        objectRenderer = GetComponentInParent<Renderer>();
        if(objectRenderer != null)
        {
            initialColor = objectRenderer.material.color;
            initialAlphaThreshold = objectRenderer.material.GetFloat(alphaClipPropertyName);
        }  
    }

    private void OnTriggerEnter(Collider other)
    {
        // Triggers the burning process if a lit match collides with the object
        if (other.gameObject.TryGetComponent<Match_Fire>(out Match_Fire matchFire))
        {
            if (matchFire.fireInstance != null && !isLighted)
            {
                Fire();
            }
        }
    }

    private void Fire()
    {
        // Starts the burning process and disables the UI
        isLighted = true;
        interactable.SetUI(!isLighted);

        StartCoroutine(Burn());
    }

    private IEnumerator FadeToAsh()
    {
        // Gradually fades the object to black and increases the alpha cutoff to simulate burning to ash
        yield return new WaitForSeconds(burningDelay);
        float elapsedTime = 0f;

        while (elapsedTime < burningDuration)
        {
            float normalizedTime = (elapsedTime) / burningDuration;

            float currentAlphaThreshold = Mathf.Lerp(initialAlphaThreshold, targetAlphaThreshold, normalizedTime);
            objectRenderer.material.SetFloat(alphaClipPropertyName, currentAlphaThreshold);

            float currentAlpha = Mathf.Lerp(initialColor.a, targetColor.a, normalizedTime);
            Color newColor = new Color(targetColor.r, targetColor.g, targetColor.b, currentAlpha);
            objectRenderer.material.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        objectRenderer.material.color = targetColor;
        objectRenderer.material.SetFloat(alphaClipPropertyName, targetAlphaThreshold);
    }

    private IEnumerator Burn()
    {
        // Plays fire effects, starts burning, and updates interaction state after burning is complete
        fireVisual.Play();
        SoundEffectManager.PlaySFXLoop(SFX);
        StartCoroutine(FadeToAsh());

        yield return new WaitForSeconds(burningDuration);

        fireVisual.Stop();
        SoundEffectManager.StopSFXLoop();

        interactable.IncreaseInteractionLevel();
    }
}