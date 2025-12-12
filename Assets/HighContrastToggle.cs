using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class HighContrastToggle : MonoBehaviour
{
    public Toggle toggle;
    public Volume volume;

    private ColorAdjustments colorAdjustments;

    void Start()
    {
        toggle.onValueChanged.AddListener(SetHighContrast);

        // Try get the actual effect from the volume
        if (volume.profile.TryGet(out colorAdjustments))
        {
            toggle.isOn = colorAdjustments.contrast.value > 0;
        }
    }

    void SetHighContrast(bool isOn)
    {
        if (colorAdjustments == null) return;

        colorAdjustments.contrast.value = isOn ? 50f : 0f;
    }
}
