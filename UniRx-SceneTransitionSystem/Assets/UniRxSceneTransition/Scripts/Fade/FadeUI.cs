using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(Mask))]
public class FadeUI : MonoBehaviour, IFade
{

    [SerializeField, Range(0, 1)]
    private float cutoutRange;

    public float Range
    {
        get { return cutoutRange; }
        set
        {
            cutoutRange = value;
            UpdateMaskCutout(cutoutRange);
        }
    }

    [SerializeField] Material material = null;
    [SerializeField] RenderTexture renderTexture = null;

    [SerializeField] Texture texture = null;

    private void UpdateMaskCutout(float range)
    {
        material.SetFloat("_Range", range);

        Graphics.Blit(texture, renderTexture, material);

        var mask = GetComponent<Mask>();
        mask.enabled = false;
        mask.enabled = true;
    }
}