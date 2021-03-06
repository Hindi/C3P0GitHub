﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Monobehaviour class used to actually resize UI Panels supposed to always have the same physical size, if that physical size is larger than the current's screen physical size
/// </summary>
public class PanelCapSize : MonoBehaviour {

    private RectTransform rect;
    private Vector2 originalSize;
    private float originalRatio;

	// Use this for initialization
	void Start () {
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
        if (rect.sizeDelta.y >= standardizedHeight * 3/4)
        {
            rect.sizeDelta = new Vector2(standardizedHeight * 3 / 4 * originalRatio, standardizedHeight * 3 / 4);
        }
        else if ((rect.sizeDelta.x < standardizedWidth * 3/4 -10) && (rect.sizeDelta.y < standardizedHeight * 3/4 -10))
        {
            rect.sizeDelta = originalSize;
        }
	}
}
