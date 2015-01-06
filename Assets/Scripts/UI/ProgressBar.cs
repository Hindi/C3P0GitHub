using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour
{
    private float currentValue; //current progress
    private float maximumValue; //current progress

    private Vector2 pos;
    private Vector2 size;

    [SerializeField]
    private Texture2D emptyTex;
    [SerializeField]
    private Texture2D fullTex;

    private Rect barRect;
    private Rect backTextRect;
    private Rect frontTextRect;
    private float factor;
    void OnGUI()
    {
        //draw the background:
        GUI.BeginGroup(barRect);
        GUI.Box(backTextRect, emptyTex);

        //draw the filled-in part:
        GUI.BeginGroup(new Rect(0, 0, size.x * fillFactor() * factor, size.y * factor));
        GUI.Box(frontTextRect, fullTex);

        GUI.EndGroup();
        GUI.EndGroup();
    }

    public void init(float maxValue, Vector2 position, Vector2 size)
    {
        maximumValue = maxValue;
        pos = position;
        this.size = size;
        factor = Responsive.responsiveFactor();

        Debug.Log(pos.x + " " + pos.y);
        barRect = new Rect(pos.x, pos.y, size.x * factor, size.y * factor);
        backTextRect = new Rect(0, 0, size.x * factor, size.y * factor);
        frontTextRect = new Rect(0, 0, size.x * factor, size.y * factor);


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
        Debug.Log(size.x * fillFactor() * factor);
    }
}