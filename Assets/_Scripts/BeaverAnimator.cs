using UnityEngine;

public class BeaverAnimator : MonoBehaviour
{
    public PlayerSprite player; // Reference to this object's PlayerSprite

    public Material idleMaterial;
    public Material movingMaterial1;
    public Material movingMaterial2;

    private MeshRenderer quadRenderer;
    private float swapTime = 0.2f;
    private bool isSwapping;
    private bool useFirstMovingMaterial = true;

    void Start()
    {
        quadRenderer = GetComponent<MeshRenderer>();
        if (quadRenderer == null)
        {
            Debug.LogError("BeaverAnimator requires a MeshRenderer on the same GameObject.");
            enabled = false;
            return;
        }

        quadRenderer.material = idleMaterial; // Start with idle material
    }

    void Update()
    {
        if (player == null) return;

        if (player.moving && !isSwapping)
        {
            StartCoroutine(SwapMovingMaterials());
        }
        else if (!player.moving)
        {
            StopAllCoroutines();
            isSwapping = false;
            quadRenderer.material = idleMaterial;
        }
    }

    private System.Collections.IEnumerator SwapMovingMaterials()
    {
        isSwapping = true;
        while (true)
        {
            quadRenderer.material = useFirstMovingMaterial ? movingMaterial1 : movingMaterial2;
            useFirstMovingMaterial = !useFirstMovingMaterial;
            yield return new WaitForSeconds(swapTime);
        }
    }
}
