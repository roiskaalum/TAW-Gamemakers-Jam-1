using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSprite : MonoBehaviour
{
    public float moveDistance = 1f; // Distance to move per key press
    private Rigidbody rb;
    public Slider slider;

    public GameObject player; // The player (Quad with 2D sprite)
    public Vector3 offset = new Vector3(0, 1, 0); // Offset from the player (optional)
    public int playerNumber = 1;

    public float fillSpeed = 1f; // How fast the progress bar fills
    private bool isFilling = false; // Tracks if the progress is active

    public bool isCarryingStick = false;

    public bool canStartProgress = false; // Determines if the progress bar can start sliding (can be controlled externally)

    public bool canDropOff = false;

    public DropOffZone dropOffZone;

    public Interactable interactable;

    public bool isInsideCollider = false;

    private SoundManager soundManager;
    public AudioType[] audioTypes = { AudioType.Soundtrack_01, AudioType.Soundtrack_02, AudioType.Soundtrack_03, AudioType.Soundtrack_04, AudioType.Soundtrack_05 };
    private int musicIndexer = 1;
    private float timeSinceLastActivated = 0;

    public bool moving = false;


    void Start()
    {
        soundManager = SoundManager.instance;
        if (soundManager == null)
        {
            Debug.Log("Something went wrong, because SoundManager Instace is null");
        }
        soundManager.PlayAudio(AudioType.Soundtrack_01); // on scene load, play soundtrack 1.

        // Get the Rigidbody component attached to this object
        rb = GetComponent<Rigidbody>();
        // Get Background and Fill Images
        Image background = slider.transform.Find("Background").GetComponent<Image>();
        Image fill = slider.transform.Find("Fill Area/Fill").GetComponent<Image>();
        GameObject handle = slider.transform.Find("Handle Slide Area/Handle").gameObject;


        // Set Colors
        background.color = Color.red;   // Background -> Red
        fill.color = Color.green;       // Fill -> Green

        // Disable the Handle
        handle.SetActive(false);
        // Ensure the slider starts at 0 and is non-interactable
        slider.value = 0;
        slider.interactable = false;

        slider.gameObject.SetActive(false);
    }

    void Update()
    {
        #region TEST

        //Debug.Log(canDropOff + " : CanDropOff");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isCarryingStick = false;
            canStartProgress = true;
            Debug.Log("Dropped Stick");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && playerNumber == 1)
        {
            Debug.Log("canDropOff: " + canDropOff + " | isCarryingStick: " + isCarryingStick + " | canStartProgress: " + canStartProgress);
        }

        #endregion TEST

        #region Transform and Rotate on Input
        Vector2 inputVector = Vector2.zero;
        Quaternion rotationDir = transform.rotation;
        if (playerNumber == 1)
        {
            if (Input.GetKey(KeyCode.W)) { inputVector.y += 1; rotationDir = Quaternion.Euler(90, 0, 0); }
            if (Input.GetKey(KeyCode.S)) { inputVector.y += -1; rotationDir = Quaternion.Euler(90, 180, 0); }
            if (Input.GetKey(KeyCode.A)) { inputVector.x += -1; rotationDir = Quaternion.Euler(90, -90, 0); }
            if (Input.GetKey(KeyCode.D)) { inputVector.x += 1; rotationDir = Quaternion.Euler(90, 90, 0); }
        }
        else
        {
            if (Input.GetKey(KeyCode.UpArrow)) { inputVector.y += 1; rotationDir = Quaternion.Euler(90, 0, 0); }
            if (Input.GetKey(KeyCode.DownArrow)) { inputVector.y += -1; rotationDir = Quaternion.Euler(90, 180, 0); }
            if (Input.GetKey(KeyCode.LeftArrow)) { inputVector.x += -1; rotationDir = Quaternion.Euler(90, -90, 0); }
            if (Input.GetKey(KeyCode.RightArrow)) { inputVector.x += 1; rotationDir = Quaternion.Euler(90, 90, 0); }
        }

        inputVector += inputVector.normalized;

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir == Vector3.zero)
        {
            moving = false;
        }
        else
        {
            moving = true;
        }


            Vector3 screenPosition = Camera.main.WorldToScreenPoint(player.transform.position + offset);
        slider.transform.position = screenPosition;

        // Move the Rigidbody using MovePosition
        rb.MovePosition(rb.position + moveDir * moveDistance * Time.deltaTime);

        // Rotate the Rigidbody using MoveRotation
        rb.MoveRotation(rotationDir);

        #endregion Transform and Rotate on Input
        //Background Music Switcher.
        if (Input.GetKey(KeyCode.T) && 1 <= Time.time - timeSinceLastActivated)
        {
            timeSinceLastActivated = Time.time;
            switchMusic();
        }
        if (Input.GetKey(KeyCode.G) && 1 <= Time.time - timeSinceLastActivated)
        {
            timeSinceLastActivated = Time.time;
            if(!(musicIndexer!=5))
                soundManager.StopAudio(audioTypes[musicIndexer - 1]);
            else
                soundManager.StopAudio(audioTypes[0]);
        }


        //Debug.Log(canStartProgress + " : " + !isCarryingStick);
        // Handle progress bar logic based on player input
        //Debug.Log(canStartProgress + " : Can StartProgress ------ Combined: " + (!isCarryingStick || canDropOff) + " ------------------------- !isCarryingStick: " + !isCarryingStick + " --- " + canDropOff + " : canDropOff");
        if ((canStartProgress && (isCarryingStick && canDropOff)) || (canStartProgress && !canDropOff))
        {
            // Check for interaction key (E for Player 1, Enter for Player 2)
            if (playerNumber == 1 && Input.GetKeyDown(KeyCode.E) && !isFilling)
            {
                Debug.Log("Made it inside the Key press and PlayerNumber conditional");
                if (canDropOff)
                {
                    soundManager.PlayAudio(AudioType.Building_The_Dam);
                }
                else if (!isCarryingStick)
                {
                    StartCoroutine(PlayChopSound());
                    
                }

                    StartProgress(); // Start the progress bar for Player 1
            }
            else if (playerNumber == 2 && Input.GetKeyDown(KeyCode.Return) && !isFilling)
            {
                Debug.Log("Made it inside the Key press and PlayerNumber conditional");
                StartProgress(); // Start the progress bar for Player 2
            }
        }

        // If progress is active, update it over time
        if (isFilling)
        {
            slider.value += fillSpeed * Time.deltaTime;

            // If progress is full, stop updating
            if (slider.value >= slider.maxValue)
            {
                isFilling = false;
                Debug.Log("Progress bar complete, isFilling set to false.");
                ProgressComplete(this);
            }
        }
    }

    private IEnumerator PlayChopSound()
    {
        soundManager.PlayAudio(AudioType.Chopping_The_Wood);
        yield return new WaitForSeconds(3);
        soundManager.PlayAudio(AudioType.Tree_Falling_Down);
    }

    private void switchMusic()
    {
        soundManager.PlayAudio(audioTypes[musicIndexer]);
        Debug.Log(audioTypes[musicIndexer]);
        musicIndexer++;
        if(musicIndexer >= 5) { musicIndexer = 0; }
    }

    private void OnCollisionExit(Collision collision)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


    // Method to start filling the progress bar
    private void StartProgress()
    {
        slider.gameObject.SetActive(true);
        isFilling = true;
        slider.value = 0; // Reset progress each time it's activated
        //Disable movement?
        
        Debug.Log("Inside StartProgress");
    }

    // Method called when progress reaches 100%
    private void ProgressComplete(PlayerSprite player)
    {
        if (canDropOff && isCarryingStick)
        {
            Debug.Log("Stick dropped off, isCarryingStick set to false.");
            int numbReceivedFromDropOffZone = dropOffZone.stageIdentifier;
            dropOffZone.DroppedAStickOff(numbReceivedFromDropOffZone);
            dropOffZone.stageIdentifier = numbReceivedFromDropOffZone + 1;
        }

        slider.value = 0;
        slider.gameObject.SetActive(false);
        
        if (canDropOff)
        {
            isCarryingStick = false;
        }
        else
        {
            interactable.HideRandomQuad(player);
        }
        
        canStartProgress = false;
        Debug.Log("canDropOff: " + canDropOff + " | isCarryingStick: " + isCarryingStick + " | canStartProgress: " + canStartProgress);
    }
}
