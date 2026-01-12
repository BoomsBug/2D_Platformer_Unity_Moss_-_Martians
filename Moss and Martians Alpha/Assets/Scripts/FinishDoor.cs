using UnityEngine;
using UnityEngine.InputSystem;

public class FinishDoor : MonoBehaviour
{
    [SerializeField] private LevelManager levelManager;
    private bool playerInRange;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void FinishStage(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (!playerInRange)
        {
            return;
        }

        if (levelManager != null)
        {
            levelManager.FinishLevel();
        }
    }
}
