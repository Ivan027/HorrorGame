using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private Dialogue _dialogue;
    [SerializeField] private Transform _lookAtPoint;
    
    private Collider _collider;
    private DialogueController _dialogueController;
    private PlayerCamera _playerCamera;

    [Inject]
    public void Construct(DialogueController dialogueController, PlayerCamera playerCamera)
    {
        _dialogueController = dialogueController;
        _playerCamera = playerCamera;
    }
    
    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _collider.enabled = false;

        _playerCamera.LookAt(_lookAtPoint);
        _dialogueController.StartDialogue(_dialogue);
        _dialogueController.OnDialogueEnded += EndDialogue;
    }
    
    private void EndDialogue()
    {
        _playerCamera.SetCameraFree();
        _dialogueController.OnDialogueEnded -= EndDialogue;
    }
}
