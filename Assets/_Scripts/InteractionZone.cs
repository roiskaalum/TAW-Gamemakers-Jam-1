using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public PlayerSprite player1;
    public PlayerSprite player2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherObject = collision.gameObject;

        PlayerSprite otherScript = otherObject.GetComponent<PlayerSprite>();

        Debug.Log("INTERACTIONZONE: " + otherScript);

        if (otherScript != null)
        {
            if (player1.playerNumber == otherScript.playerNumber)
            {
                //Player 1 collided.
                Debug.Log("Player1 Collided");
                player1.canStartProgress = true;
            }

            if (player2.playerNumber == otherScript.playerNumber)
            {
                //Player 2 collided.
                Debug.Log("Player2 Collided");
                player2.canStartProgress = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Disable the text, and change the color back.
        GameObject otherObject = collision.gameObject;

        PlayerSprite otherScript = otherObject.GetComponent<PlayerSprite>();

        if (otherScript != null)
        {
            if (player1.playerNumber == otherScript.playerNumber)
            {
                //Player 1 collided.
                player1.canStartProgress = false;
            }

            if (player2.playerNumber == otherScript.playerNumber)
            {
                //Player 2 collided.
                player2.canStartProgress = false;
            }
        }
    }
}
