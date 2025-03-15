using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player1; // Reference to Player 1's transform
    public Transform player2; // Reference to Player 2's transform
    public float zOffset = 0f; // The desired Z offset for the camera's position
    public float smoothSpeed = 0.125f; // Smoothness factor for following the players

    private Vector3 velocity = Vector3.zero; // Used for smoothing
    private float initialX; // Initial X position of the camera
    private float initialY; // Initial Y position of the camera

    void Start()
    {
        // Store the initial X and Y positions of the camera
        initialX = transform.position.x;
        initialY = transform.position.y;
    }

    void Update()
    {
        // Ensure both players are assigned
        if (player1 != null && player2 != null)
        {
            // Calculate the center point between both players on the X and Y axes
            Vector3 centerPosition = (player1.position + player2.position) / 2;

            // Set the z-position based on the average of the players, while preserving the initial x and y positions
            centerPosition.x = initialX;
            centerPosition.y = initialY;
            centerPosition.z = Mathf.Lerp(transform.position.z, centerPosition.z, smoothSpeed); // Smoothly update the z-position

            // Move the camera to the new position
            transform.position = centerPosition;
        }
    }
}
