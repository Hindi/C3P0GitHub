using UnityEngine;
using System.Collections;

public class Frightening : MonoBehaviour {

	void OnDestroy(){
		SendMessageUpwards("frighten", SendMessageOptions.DontRequireReceiver);
	}
}
