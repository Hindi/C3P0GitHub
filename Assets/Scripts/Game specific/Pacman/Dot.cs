using UnityEngine;
using System.Collections;

public class Dot : MonoBehaviour {


	void OnDestroy(){
		EventManager.Raise(EnumEvent.DOT_EATEN);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
