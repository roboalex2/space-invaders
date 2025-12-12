using UnityEngine;
using TMPro;

public class UIAccessibilityTextScaler : MonoBehaviour
{
    public float step = 2f;
    public float minSize = 12f;
    public float maxSize = 40f;

    private TMP_Text[] allTexts;

    void Start()
    {
        allTexts = FindObjectsOfType<TMP_Text>(true);   
    }

    public void IncreaseTextSize()
    {
        foreach (var txt in allTexts)
        {
            float newSize = Mathf.Clamp(txt.fontSize + step, minSize, maxSize);
            txt.fontSize = newSize;
        }
    }

    public void DecreaseTextSize()
    {
        foreach (var txt in allTexts)
        {
            float newSize = Mathf.Clamp(txt.fontSize - step, minSize, maxSize);
            txt.fontSize = newSize;
        }
    }
}