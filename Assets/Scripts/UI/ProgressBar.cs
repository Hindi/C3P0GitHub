using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour
{
    private float currentValue; //current progress
    private float maximumValue; //current progress
    [SerializeField]
    private Vector2 pos;
    [SerializeField]
    private Vector2 size;
    [SerializeField]
    private Texture2D emptyTex;
    [SerializeField]
    private Texture2D fullTex;

    void OnGUI()
    {
        //draw the background:
        GUI.BeginGroup(new Rect(pos.x, pos.y, size.x, size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), emptyTex);

        //draw the filled-in part:
        GUI.BeginGroup(new Rect(0, 0, size.x * fillFactor(), size.y));
        GUI.Box(new Rect(0, 0, size.x, size.y), fullTex);
        GUI.EndGroup();
        GUI.EndGroup();
    }

    public void init(float maxValue, Vector2 position, Vector2 size)
    {
        maximumValue = maxValue;
        pos = position;
        this.size = size;
    }

    private float fillFactor()
    {
        return (currentValue / maximumValue);
    }

    public void updateValue(float v)
    {
        currentValue = v;
    }

    void Update()
    {

    }
}