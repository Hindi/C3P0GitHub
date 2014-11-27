using UnityEngine;
using System.Collections;

/**
 * One of the game states, will probably require another level of inheritence.
 * 
 */

public class GameState : State
{

    private GameObject cube_;

    public GameState(StateManager stateManager) : base(stateManager)
    {
        cube_ = GameObject.FindGameObjectWithTag("Cube");
    }

    // Use this for initialization
    public override void start()
    {
        Debug.Log("It's me, GAME");
    }

    // Use this for state transition
    public override void end()
    {

    }

    // Update is called once per frame
    public override void update()
    {

    }

    public override void noticeInput(KeyCode key)
    {
        if (key == KeyCode.Space)
            cube_.renderer.material.color = Color.red;
        if (key == KeyCode.Return)
        {
            EventManager<string>.Raise(EnumEvent.LOADLEVEL, "StudMenu");
            stateManager_.changeState(StateEnum.MENU);
        }
    }
}
