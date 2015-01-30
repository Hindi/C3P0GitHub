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
            stateManager.noticeInput(EnumInput.LEFTDOWN);
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
            stateManager.noticeInput(EnumInput.LEFTUP);
        else if (Input.GetKey(KeyCode.LeftArrow))
            stateManager.noticeInput(EnumInput.LEFT);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            stateManager.noticeInput(EnumInput.RIGHTDOWN);
        else if (Input.GetKey(KeyCode.RightArrow))
            stateManager.noticeInput(EnumInput.RIGHT);
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            stateManager.noticeInput(EnumInput.RIGHTUP);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            stateManager.noticeInput(EnumInput.UPDOWN);
        else if (Input.GetKey(KeyCode.UpArrow))
            stateManager.noticeInput(EnumInput.UP);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            stateManager.noticeInput(EnumInput.DOWNDOWN);
        else if (Input.GetKey(KeyCode.DownArrow))
            stateManager.noticeInput(EnumInput.DOWN);
        if (Input.GetKeyDown(KeyCode.Space))
            stateManager.noticeInput(EnumInput.SPACE);
        if (Input.GetKeyDown(KeyCode.Return))
            stateManager.noticeInput(EnumInput.RETURN);
        if (Input.GetKeyDown(KeyCode.Escape))
            stateManager.noticeInput(EnumInput.ESCAPE);
        if (Input.GetKeyDown(KeyCode.Tab))
            stateManager.noticeInput(EnumInput.TAB);
        if (Input.GetKeyDown(KeyCode.Menu))
            stateManager.noticeInput(EnumInput.MENU);

        if (Application.isMobilePlatform)
        {
            if (Input.touches.Length > 0)
            {
                stateManager.noticeInput(EnumInput.TOUCH, Input.touches);
            }
        }
	}
}
