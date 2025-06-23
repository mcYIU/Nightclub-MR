using Oculus.Interaction;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Interactable defines objects in the game world that the player can interact with (grab, poke, etc.).
/// It manages enabling/disabling interactions, UI feedback, and interaction progression.
/// </summary>

public class Interactable : MonoBehaviour
{
    public enum InteractableType
    {
        Grab,
        Poke,
        None
    }

    public InteractionManager interactionManager;
    [SerializeField] private InteractableType type;
    [SerializeField] private Canvas[] interactionUI;
    [HideInInspector] public bool isInteractionEnabled;

    [SerializeField] private Grabbable[] grabbables;
    [SerializeField] private PokeInteractable[] pokeInteractables;

    private int interactionLevel;

    public void SetInteraction(bool isEnabled)
    {
        // Enable or disable interaction components based on the type (Grab, Poke, None)
        switch (type)
        {
            case InteractableType.None:
                break;
            case InteractableType.Grab:
                foreach (var _grab in grabbables) _grab.enabled = isEnabled;
                break;
            case InteractableType.Poke:
                foreach (var _poke in pokeInteractables) _poke.enabled = isEnabled;
                break;
        }

        SetUI(isEnabled);

        isInteractionEnabled = isEnabled;
    }

    public void SetInteractionLevel(int level)
    {
        // Set the interaction level for progression tracking
        interactionLevel = level;
    }

    public void SetUI(bool isActive)
    {
        // Show or hide the interaction UI elements
        foreach (Canvas c in interactionUI)
        {
            if (c != null) c.enabled = isActive;
        }
    }

    public void IncreaseInteractionLevel()
    {
        // Called when interaction is completed; advances the interaction level in the manager
        SetUI(false);

        if (interactionManager.LevelIndex == interactionLevel)
        {
            int _i = interactionLevel + 1;
            interactionManager.ChangeLevelIndex(_i);
        }
    }

    public void PlaySFX(AudioSource source, AudioClip clip)
    {
        // Play a sound effect for interaction feedback
        source.PlayOneShot(clip);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    SerializedProperty manager;
    SerializedProperty ui;
    SerializedProperty typeProp;
    SerializedProperty grabbablesProp;
    SerializedProperty pokeInteractablesProp;

    void OnEnable()
    {
        manager = serializedObject.FindProperty("interactionManager");
        ui = serializedObject.FindProperty("interactionUI");
        typeProp = serializedObject.FindProperty("type");
        grabbablesProp = serializedObject.FindProperty("grabbables");
        pokeInteractablesProp = serializedObject.FindProperty("pokeInteractables");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(manager);
        EditorGUILayout.PropertyField(ui);
        EditorGUILayout.PropertyField(typeProp);

        Interactable.InteractableType currentType = (Interactable.InteractableType)typeProp.enumValueIndex;

        // Handle GUI on inspector in different interactable types 
        switch (currentType)
        {
            case Interactable.InteractableType.Grab:
                EditorGUILayout.PropertyField(grabbablesProp, true);
                pokeInteractablesProp.isExpanded = false;
                break;
            case Interactable.InteractableType.Poke:
                EditorGUILayout.PropertyField(pokeInteractablesProp, true);
                grabbablesProp.isExpanded = false;
                break;
            case Interactable.InteractableType.None:
                grabbablesProp.isExpanded = false;
                pokeInteractablesProp.isExpanded = false;
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif