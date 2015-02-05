using UnityEngine;
using System.Collections;

public class PanelCapSize : MonoBehaviour {

    private static float ratio;
    private RectTransform rect;
    private Vector2 originalSize;

	// Use this for initialization
	void Start () {
        ratio = (float) Screen.width / Screen.height;
        rect = gameObject.GetComponent<RectTransform>();
        originalSize = rect.sizeDelta;
	}
	
	// Update is called once per frame
    void Update()
    {

        float dpi = 96; // default
        float standardizedWidth = Screen.width * dpi / Screen.dpi;
        float standardizedHeight = Screen.height * dpi / Screen.dpi;

        if (rect.sizeDelta.x >= standardizedWidth * 3/4)
        {
            rect.sizeDelta = new Vector2(standardizedWidth * 3/4, rect.sizeDelta.y);
        }
        else
        {
            rect.sizeDelta = new Vector2(originalSize.x, rect.sizeDelta.y);
        }

        if (rect.sizeDelta.y >= standardizedHeight * 3/4)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, standardizedHeight * 3/4);
        }
        else
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, originalSize.y);
        }
	}
}
