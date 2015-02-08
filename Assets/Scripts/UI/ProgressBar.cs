using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgressBar : MonoBehaviour
{
    private float currentValue;
    private float maximumValue;

    private Vector2 size;

    [SerializeField]
    private Image background;
    [SerializeField]
    private Image front;

    public void init(float maxValue)
    {
        maximumValue = maxValue;
        size = new Vector2(background.rectTransform.rect.width, background.rectTransform.rect.height);
    }

    private float fillFactor()
    {
        return (currentValue / maximumValue);
    }

    public void updateValue(float v)
    {
        currentValue = v;
        if (currentValue > maximumValue)
            currentValue = maximumValue;
        front.rectTransform.sizeDelta = new Vector2(size.x * fillFactor(), front.rectTransform.sizeDelta.y);
    }
}