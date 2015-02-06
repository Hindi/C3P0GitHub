using UnityEngine;
using System.Collections;

public class PanelCapSize : MonoBehaviour {

    private static float ratio;
    private RectTransform rect;
    private Vector2 originalSize;
    private float originalRatio;

	// Use this for initialization
	void Start () {
        ratio = (float) Screen.width / Screen.height;
        rect = gameObject.GetComponent<RectTransform>();
        originalSize = rect.sizeDelta;
        originalRatio = rect.sizeDelta.x / rect.sizeDelta.y;
	}
	
	// Update is called once per frame
    void Update()
    {
        float dpi = 96; // default
        float standardizedWidth = Screen.width * dpi / ((Screen.dpi == 0) ? 96 : Screen.dpi);
        float standardizedHeight = Screen.height * dpi / ((Screen.dpi == 0) ? 96 : Screen.dpi);

        if (rect.sizeDelta.x >= standardizedWidth * 3/4)
        {
            rect.sizeDelta = new Vector2(standardizedWidth * 3 / 4, standardizedWidth * 3 / 4 * 1 / originalRatio);
        }
        else if (rect.sizeDelta.y >= standardizedHeight * 3/4)
        {
            rect.sizeDelta = new Vector2(standardizedHeight * 3 / 4 * originalRatio, standardizedHeight * 3 / 4);
        }
        else if ((rect.sizeDelta.x >= standardizedWidth * 3/4 -10) && (rect.sizeDelta.y >= standardizedHeight * 3/4 -10))
        {
            rect.sizeDelta = originalSize;
        }
	}
}
