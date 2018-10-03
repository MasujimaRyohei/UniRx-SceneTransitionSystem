using UnityEngine;
using UnityEngine.UI;

public class FadeImage : Graphic,IFade
{
    [SerializeField]
    private Texture maskTexture = null;

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

    protected override void Start()
    {
        base.Start();
        UpdateMaskTexture(maskTexture);
    }

    private void UpdateMaskCutout(float range)
    {
        enabled = true;
        material.SetFloat("_Range", 1 - range);

        if (range <= 0)
        {
            enabled = false;
        }
    }

    public void UpdateMaskTexture(Texture texture)
    {
        material.SetTexture("_MaskTex", texture);
        material.SetColor("_Color", color);
    }

#if UNITY_EDITOR
    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateMaskCutout(Range);
        UpdateMaskTexture(maskTexture);
    }
#endif
}