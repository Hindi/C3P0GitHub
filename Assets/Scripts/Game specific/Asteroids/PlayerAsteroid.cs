using UnityEngine;
using System.Collections;
using System.Collections.Generic;




public class PlayerAsteroid : MonoBehaviour {

    [SerializeField]
    private List<Transform> targetList;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Vector3 getTarget()
    {
        return targetList[(int)Random.Range(0, targetList.Count)].position;
    }
}
