using UnityEngine;
using System.Collections;

public class HandleAttack : MonoBehaviour
{
    public int playerNumber = 1;            // Player number (1 for Player 1, 2 for Player 2)
    public GameObject playerQuad;           // Reference to the specific player's quad
    public float moveDistance = 0.55f;       // Distance to move on the Y-axis
    public float moveTime = 0.2f;           // Time to move forward & back
    public float cooldownTime = 0.7f;       // Cooldown before next activation

    private bool isCooldown = false;        // Prevent spamming

    void Start()
    {
        // Set the playerQuad based on the player number in the editor or assign manually
        if (playerNumber == 1)
        {
            playerQuad = this.gameObject; // Player 1's Quad is the current object
        }
        // If Player 2, the quad is specified manually in the editor.
    }

    void Update()
    {
        if (playerNumber == 1 && Input.GetKeyDown(KeyCode.Q) && !isCooldown) // Player 1 presses Q
        {
            StartCoroutine(MoveAndCooldown());
        }
        else if (playerNumber == 2 && Input.GetKeyDown(KeyCode.RightShift) && !isCooldown) // Player 2 presses Right Control
        {
            StartCoroutine(MoveAndCooldown());
        }
    }

    IEnumerator MoveAndCooldown()
    {
        isCooldown = true;

        // Move forward on the Y-axis (positive direction)
        Vector3 startPosition = playerQuad.transform.localPosition;
        Vector3 targetPosition = new Vector3(0, moveDistance, 0.05f); // Move to 0.7f on the Y-axis, 0.05 on Z

        yield return StartCoroutine(LerpPosition(startPosition, targetPosition, moveTime / 2));

        // Return to original position
        yield return StartCoroutine(LerpPosition(targetPosition, startPosition, moveTime / 2));

        // Cooldown before allowing another activation
        yield return new WaitForSeconds(cooldownTime);

        isCooldown = false;
    }

    IEnumerator LerpPosition(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            playerQuad.transform.localPosition = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerQuad.transform.localPosition = end; // Ensure exact position
    }
}
