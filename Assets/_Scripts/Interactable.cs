using System.Globalization;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for UI elements

public class Interactable : MonoBehaviour
{
    public GameObject interactText;  // Reference to the UI Text (Interact label)
    public GameObject[] quads; // Reference to the quads to hide or show
    public PlayerSprite player1; // Reference to Player 1 (you can drag and drop in the editor)
    public PlayerSprite player2; // Reference to Player 2 (you can drag and drop in the editor)

    private int numberOfLogs = 0;

    private void Start()
    {
        // Ensure that the "Interact" text starts hidden
        if (interactText != null)
        {
            interactText.SetActive(false);
        }

        // Ensure quads are visible at the start
        foreach (var quad in quads)
        {
            quad.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactText != null)
        {
            interactText.gameObject.SetActive(true);
        }

        PlayerSprite player = other.gameObject.GetComponent<PlayerSprite>();

        if (player != null)
        {
            Debug.Log("player made from collision object wasn't null");
            Debug.Log("Colliding Player was player " + player.playerNumber);
            player.canStartProgress = true;
            player.isInsideCollider = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Hide the "Interact" text when player leaves the collider
        PlayerSprite player = other.gameObject.GetComponent<PlayerSprite>();

        if (player != null)
        {
            if (interactText != null)
            {
                interactText.gameObject.SetActive(false);
            }
            player.canStartProgress = false;
            player.isInsideCollider = false;
        }
    }

    public void HideRandomQuad(PlayerSprite player)
    {
        if (numberOfLogs <= 0)
        {
            for (int i = 0; i < 5; i++)
            {
                quads[i].SetActive(true);
            }
            numberOfLogs = 5;
            player.canStartProgress = true;
            return;
        }
        // Randomly disable one of the quads

        int randomIndex;
        int maxAttempts = 100;
        do
        {
            randomIndex = Random.Range(0, quads.Length);
            maxAttempts--;
        } while (!quads[randomIndex].activeSelf && maxAttempts <= 0);
        
        quads[randomIndex].SetActive(false);
        numberOfLogs--;
        player.isCarryingStick = true;
    }
}
