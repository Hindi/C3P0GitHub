using UnityEngine;
using System.Collections;

public class MarioFlagPole : MonoBehaviour {

    [SerializeField]
    private Transform flagBasePosition;
    private Vector3 objPosition;
    [SerializeField]
    private Transform flag;

    private bool finishing;

	// Use this for initialization
	void Start () {
	    objPosition = new Vector3(flag.position.x, flagBasePosition.position.y + 1, flag.position.z);
	}
	
	// Update is called once per frame
    void Update()
    {
        if (finishing)
        {
            flag.position = Vector3.MoveTowards(flag.position, objPosition, 5 * Time.deltaTime);
            if (Vector3.Distance(flag.position, objPosition) < 0.1f)
                finishing = false;
        }
	
	}

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player")
        {
            finishing = true;
        }
    }
}
