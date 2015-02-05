using UnityEngine;
using System.Collections;

public class Frightening : MonoBehaviour {

	void OnDestroy(){
		EventManager.Raise(EnumEvent.FRIGHTENED);
	}
}
