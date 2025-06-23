using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// GameManager is the central controller for the game's state, scene transitions, and overall progression.
/// </summary>

public class GameManager : MonoBehaviour
{
    [Serializable]
    private struct InteractionConfig
    {
        public InteractionManager[] managers;
    }

    [Serializable]
    private struct TransitionConfig
    {
        public GameLevelTrigger levelTrigger;     
        public OVRPassthroughLayer layers;
        public float fadeDuration;
        public float interval;
    }

    public static GameManager Instance { get; private set; }
    public static bool IsStarted { get; set; }
    public static bool IsCompleted { get; private set; }

    [Header("Configurations")]
    [SerializeField] private InteractionConfig interactionConfig;
    [SerializeField] private TransitionConfig transitionConfig;

    private static int gameSceneIndex;
    private const float FADE_END_VALUE = 0f;

    #region Initialization

    private void Awake()
    {
        InitializeSingleton();
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeGame()
    {
        // Subscribe to scene loaded event and perform any initial setup
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    #endregion

    #region Game State Management

    public static void CheckGameState()
    {
        // Check if all interaction managers have completed their interactions to end the level
        if (!HasInteractionManagers()) return;
        if (AreAllInteractionsCompleted())
        {
            EndLevel();
        }
    }

    private static bool HasInteractionManagers()
    {
        // Check if there are any interaction managers configured
        return Instance.interactionConfig.managers.Length > 0;
    }

    private static bool AreAllInteractionsCompleted()
    {
        // Check if all interaction managers have reached the end of their interaction layers
        int completedCount = 0;
        foreach (var manager in Instance.interactionConfig.managers)
        {
            if (manager.LevelIndex == manager.interactionLayers.Length)
            {
                completedCount++;
            }
        }
        return completedCount == Instance.interactionConfig.managers.Length;
    }

    private static void EndLevel()
    {
        // End the current level, trigger dialogue end, fade out passthrough, and enable level trigger
        if (gameSceneIndex == 0)
        {
            IsCompleted = true;
            DialogueManager.EndDialogue();
            Instance.StartCoroutine(Instance.ChangePassThroughOpacity());
            Instance.EnableLevelTrigger();
        }
    }

    #endregion

    #region Scene Management

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Update the current scene index when a new scene is loaded
        gameSceneIndex = scene.buildIndex;
    }

    public static void ChangeToNextScene(int sceneIncrement = 1)
    {
        Instance.StartCoroutine(Instance.PerformSceneTransition(sceneIncrement));
    }

    private IEnumerator PerformSceneTransition(int sceneIncrement = 1)
    {
        // Handle music and perform the actual scene change with fade effects
        HandleTransitionMusic();
        yield return ExecuteSceneChange(sceneIncrement);
    }

    private void HandleTransitionMusic()
    {
        // Stop music and adjust interval if not in the 1st scene
        if (gameSceneIndex != 0) 
        {
            MusicManager.StopMusic();
            transitionConfig.interval += 2.0f;
        }   
    }

    private void EnableLevelTrigger()
    {
        // Enable the trigger point for the next level or event
        if(transitionConfig.levelTrigger) transitionConfig.levelTrigger.EnableTriggerPoint();
    }

    private IEnumerator ExecuteSceneChange(int sceneIncrement = 1)
    {
        // Fade out, wait, load the next scene, then fade in
        FaderController.FadeOut();
        yield return new WaitForSeconds(transitionConfig.interval);
        SceneManager.LoadScene(gameSceneIndex + sceneIncrement);
        yield return new WaitForSeconds(transitionConfig.interval);
        FaderController.FadeIn();
    }

    #endregion

    #region Visual Effects

    private IEnumerator ChangePassThroughOpacity()
    {
        // Gradually fade out the passthrough layer's opacity for visual effect
        float startValue = transitionConfig.layers.textureOpacity;
        float elapsedTime = 0f;
        while (elapsedTime < transitionConfig.fadeDuration)
        {
            transitionConfig.layers.textureOpacity = Mathf.Lerp(
                startValue,
                FADE_END_VALUE,
                elapsedTime / transitionConfig.fadeDuration
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transitionConfig.layers.textureOpacity = FADE_END_VALUE;
    }

    #endregion

    private void Update()
    {
        // Debug controls for ending the level or skipping scenes in the 1st scene
        if (gameSceneIndex == 0 && !GameManager.IsCompleted)
        {
            if (OVRInput.GetDown(OVRInput.RawButton.B)) EndLevel();  
            if (OVRInput.GetDown(OVRInput.RawButton.Y)) GameManager.ChangeToNextScene(2);
        }
    }
}