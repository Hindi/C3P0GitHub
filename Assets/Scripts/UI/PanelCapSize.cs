using UnityEngine;
using System.Collections;

public class PanelCapSize : MonoBehaviour {

    private RectTransform rect;
    private Vector2 size;

	// Use this for initialization
	void Start () {
        rect = gameObject.GetComponent<RectTransform>();
        size = rect.sizeDelta;
	}
	
	// Update is called once per frame
    void Update()
    {
        if (rect.sizeDelta.x > Screen.width)
        {
            rect.sizeDelta = new Vector2(Screen.width, rect.sizeDelta.y);
        }
        else
        {
            rect.sizeDelta = new Vector2(size.x, rect.sizeDelta.y);
        }

        if (rect.sizeDelta.y > Screen.height)
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, Screen.height);
        }
        else
        {
            rect.sizeDelta = new Vector2(rect.sizeDelta.x, size.y);
        }
	}
}
