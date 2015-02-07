using UnityEngine;
using System.Collections;

public class ControlIcon : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.SetActive(Application.isMobilePlatform);
	}
}
