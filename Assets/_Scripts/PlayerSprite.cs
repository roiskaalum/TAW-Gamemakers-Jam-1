using UnityEngine;
using UnityEngine.UI;

public class PlayerSprite : MonoBehaviour
{
    public float moveDistance = 1f; // Distance to move per key press
    private Rigidbody rb;
    public Slider slider;

    public GameObject player; // The player (Quad with 2D sprite)
    public Vector3 offset = new Vector3(0, 1, 0); // Offset from the player (optional)

    void Start()
    {
        // Get the Rigidbody component attached to this object
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        Quaternion rotationDir = transform.rotation;

        if (Input.GetKey(KeyCode.W)) { inputVector.y += 1; rotationDir = Quaternion.Euler(90, 0, 0); }
        if (Input.GetKey(KeyCode.S)) { inputVector.y += -1; rotationDir = Quaternion.Euler(90, 180, 0); }
        if (Input.GetKey(KeyCode.A)) { inputVector.x += -1; rotationDir = Quaternion.Euler(90, -90, 0); }
        if (Input.GetKey(KeyCode.D)) { inputVector.x += 1; rotationDir = Quaternion.Euler(90, 90, 0); }

        inputVector += inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        Vector3 screenPosition = Camera.main.WorldToScreenPoint(player.transform.position + offset);
        slider.transform.position = screenPosition;

        // Move the Rigidbody using MovePosition
        rb.MovePosition(rb.position + moveDir * moveDistance * Time.deltaTime);

        // Rotate the Rigidbody using MoveRotation
        rb.MoveRotation(rotationDir);



        Debug.Log(rb.position + " : " + rb.rotation);
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
