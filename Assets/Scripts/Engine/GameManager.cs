using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private bool isPlayerFrozen = false;

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

    void Start()
    {
        Debug.Log("Hello World!");
    }

    public void TogglePlayerFreeze()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement not found in scene");
            return;
        }
        playerMovement.ToggleFreezeMovement();
    }


}
