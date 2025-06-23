using Oculus.Interaction.Body.PoseDetection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DanceQuestManager controls the dance quest logic, including pose detection, quest progression, and UI instructions for the player
/// </summary>

public class DanceQuestManager : MonoBehaviour
{
    [Header("Pose")]
    [SerializeField] private BodyPoseComparerActiveState[] poses;
    [Header("Customer Dialogue")]
    [SerializeField] private CustomerDialogueTrigger customerDialogue;
    [Header("Instruction UI")]
    [SerializeField] private Image noticeUI;
    [SerializeField] private TextMeshProUGUI textUI;
    [SerializeField] private string instructionText;

    [HideInInspector] public bool isActive = false;
    private int numCompletedPoses = 0;

    private void Start()
    {
        // Initialize the quest state based on whether it is active
        ResetQuest(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        // When the player enters the trigger, activate the quest if not completed
        if (other.gameObject.CompareTag("Player") && !IsQuestCompleted())
        {
            isActive = !isActive;
            ActiveInstruction(isActive);
            NextQuest();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // When the player exits the trigger, deactivate the quest if not completed
        if (other.gameObject.CompareTag("Player") && !IsQuestCompleted())
        {
            isActive = !isActive;
            ActiveInstruction(isActive);
            ResetQuest(isActive);
        }
    }

    private bool IsQuestCompleted()
    {
        // Check if all poses have been completed
        return numCompletedPoses == poses.Length;
    }

    private void ResetQuest(bool _isActive)
    {
        // Disable all pose comparers and set their active state
        foreach (var p in poses) 
        {
            p.enabled = false;
            p.gameObject.SetActive(_isActive);
        }
    }

    private void NextQuest()
    {
        // Prepare the next pose in the quest
        ResetQuest(isActive);
        poses[numCompletedPoses].enabled = true;
    }

    private void ActiveInstruction(bool _isActive)
    {
        // Show or hide the instruction UI based on quest state
        noticeUI.enabled = IsQuestCompleted() ? _isActive : !_isActive;
        textUI.enabled = _isActive;
        textUI.text = instructionText;
    }

    public void CheckPoseCompletion()
    {
        // Called when a pose is completed; advances quest or triggers dialogue
        numCompletedPoses++;
        if (IsQuestCompleted())
        {
            if (customerDialogue) customerDialogue.Talk();
            isActive = !isActive;
            ActiveInstruction(isActive);
            ResetQuest(isActive);
        }
        else
            NextQuest();
    }
}
