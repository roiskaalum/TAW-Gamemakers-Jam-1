using System.Collections;
using TMPro;
using UnityEngine;

public class DropOffZone : MonoBehaviour
{
    public GameObject[] rivers;
    public GameObject[] dams;
    public GameObject initialRiver;
    public int stageIdentifier = 0;

    public TextMeshPro textObject;
    public float displayTime = 2f;


    

    private void Start()
    {
        foreach (GameObject item in rivers)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in dams)
        {
            item.SetActive(false);
        }

        initialRiver.SetActive(true);
        StartCoroutine(ShowTextTemporarily());
    }

    private IEnumerator ShowTextTemporarily()
    {
        textObject.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayTime);
        textObject.gameObject.SetActive(false);
    }

    public void DroppedAStickOff(int stageIdentifier)
    {
        Debug.Log("Identifier: " +  stageIdentifier);
        if (stageIdentifier >= 3)
        {
            Debug.Log("Last Stage Reached.");
            return;
        }

        if(stageIdentifier == 0)
        {
            initialRiver.SetActive(false);
            rivers[stageIdentifier].SetActive(true);
            dams[stageIdentifier].SetActive(true);
            return;
        }

        rivers[stageIdentifier-1].SetActive(false);
        //dams[stageIdentifier-1].SetActive(false); // Dams should actually never be unabled when they've been enabled once.

        rivers[stageIdentifier].SetActive(true);
        dams[stageIdentifier].SetActive(true);

        stageIdentifier++;

        //First time around, set initialriver inactive, set first stage of dam and river active. after the 3. item in the array ahs been set active, return from this method, always.

    }

    #region canDropOffCheck
    private void OnTriggerEnter(Collider other)
    {
        PlayerSprite player = other.gameObject.GetComponent<PlayerSprite>();
        if (player != null)
        {
            player.canDropOff = true;
            player.canStartProgress = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerSprite player = other.gameObject.GetComponent<PlayerSprite>();
        if (player != null)
        {
            player.canDropOff = false;
            player.canStartProgress = false;
        }
    }
    #endregion canDropOffCheck
}
