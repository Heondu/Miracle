using UnityEngine;

public class Transparent : MonoBehaviour
{
    [SerializeField] private Material transparentMaterial;

    private new Renderer renderer;
    private Material originMaterial;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        originMaterial = renderer.material;
    }

    public void ShowTransparent()
    {
        renderer.material = transparentMaterial;
    }

    public void ShowSolid()
    {
        renderer.material = originMaterial;
    }
}
