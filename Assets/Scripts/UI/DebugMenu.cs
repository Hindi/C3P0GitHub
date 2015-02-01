using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>Class mostly used to debug onmobile phone. It prints things on screen.</summary>
public class DebugMenu : MonoBehaviour {

    [SerializeField]
    private Text debugLabel;

	// Use this for initialization
	void Start () {
	    EventManager<string>.AddListener(EnumEvent.PRINTONSCREEN, onPrintOnScreen);
	}

    void onPrintOnScreen(string s)
    {
        debugLabel.text = s;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
