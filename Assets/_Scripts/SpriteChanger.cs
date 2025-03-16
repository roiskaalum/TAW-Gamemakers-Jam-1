using UnityEngine;

public class SpriteChanger : MonoBehaviour
{
    public float swapTime = 1.0f; // Time between swaps
    private GameObject quad1;
    private GameObject quad2;
    private bool isQuad1Active = true;

    void Start()
    {
        if (transform.childCount < 2)
        {
            Debug.LogError("SpriteChanger requires exactly two child quads.");
            enabled = false;
            return;
        }

        quad1 = transform.GetChild(0).gameObject;
        quad2 = transform.GetChild(1).gameObject;

        InvokeRepeating(nameof(SwapQuads), swapTime, swapTime);
    }

    void SwapQuads()
    {
        isQuad1Active = !isQuad1Active;
        quad1.SetActive(isQuad1Active);
        quad2.SetActive(!isQuad1Active);
    }
}
