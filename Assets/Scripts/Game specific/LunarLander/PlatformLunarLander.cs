using UnityEngine;
using System.Collections;

public class PlatformLunarLander : MonoBehaviour {

    [SerializeField]
    private Sprite platformWithLight;
    [SerializeField]
    private Sprite platformWithoutLight;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void lightUp()
    {
        GetComponent<SpriteRenderer>().sprite = platformWithLight;
    }

    public void lightDown()
    {
        GetComponent<SpriteRenderer>().sprite = platformWithoutLight;
    }
}
