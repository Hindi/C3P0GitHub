using UnityEngine;
using System.Collections;


/**
 * This class forwards all of the inputs to the StateManager.
 * 
 */

public class InputManager : MonoBehaviour {

    private StateManager stateManager;

	// Use this for initialization
	void Start () {
        stateManager = GameObject.FindGameObjectWithTag("StateManager").GetComponent<StateManager>();
	}
	
	// Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            stateManager.noticeInput(KeyCode.LeftArrow);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            stateManager.noticeInput(KeyCode.RightArrow);
	    if(Input.GetKeyDown(KeyCode.Space))
            stateManager.noticeInput(KeyCode.Space);
        if (Input.GetKeyDown(KeyCode.Return))
            stateManager.noticeInput(KeyCode.Return);
	}
}
