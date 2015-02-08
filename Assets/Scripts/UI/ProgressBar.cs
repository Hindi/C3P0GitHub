using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Script attached to the progress bar to make it tell the time left
/// </summary>
public class ProgressBar : MonoBehaviour
{
    private float currentValue;
    private float maximumValue;

    private Vector2 size;

    [SerializeField]
    private Image background;
    [SerializeField]
    private Image front;

    /// <summary>
    /// Initializes the size of the bar to max
    /// </summary>
    /// <param name="maxValue">The total time a question lasts</param>
    public void init(float maxValue)
    {
        maximumValue = maxValue;
        size = new Vector2(background.rectTransform.rect.width, background.rectTransform.rect.height);
    }

    /// <summary>
    /// The ratio of current time / total time
    /// </summary>
    /// <returns>Ratio between 0 and 1</returns>
    private float fillFactor()
    {
        return (currentValue / maximumValue);
    }

    /// <summary>
    /// Updates the size of the bar according to the current time
    /// </summary>
    /// <param name="v">The current time left on the question</param>
    public void updateValue(float v)
    {
        currentValue = v;
        if (currentValue > maximumValue)
            currentValue = maximumValue;
        front.rectTransform.sizeDelta = new Vector2(size.x * fillFactor(), front.rectTransform.sizeDelta.y);
    }
}