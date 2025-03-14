using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    public float moveDistance = 1f; // Distance to move per key press

    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        Quaternion rotationDir = transform.rotation;

        if (Input.GetKey(KeyCode.W)) { inputVector.y = 1; rotationDir = Quaternion.Euler(90, 0, 0); }
        if (Input.GetKey(KeyCode.S)) { inputVector.y = -1; rotationDir = Quaternion.Euler(90, 180, 0); }
        if (Input.GetKey(KeyCode.A)) { inputVector.x = -1; rotationDir = Quaternion.Euler(90, -90, 0); }
        if (Input.GetKey(KeyCode.D)) { inputVector.x = 1; rotationDir = Quaternion.Euler(90, 90, 0); }

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        
        transform.position += moveDir * moveDistance * Time.deltaTime;

        transform.rotation = rotationDir;

        Debug.Log(transform.position + " : " + transform.rotation);
        
    }
}
