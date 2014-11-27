using UnityEngine;
using System.Collections;


class SpaceInvaderState : State
{
    [SerializeField]
    private GameObject player_;
    private Player playerScript_;

    public SpaceInvaderState(StateManager stateManager)
        : base(stateManager)
    {
    }

    // Use this for initialization
    public override void start()
    {
        player_ = GameObject.FindGameObjectWithTag("Player");
        playerScript_ = player_.GetComponent<Player>();
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
        /* if (key == KeyCode.LeftArrow)
             playerScript_.setDirection(false);
         else if (key == KeyCode.RightArrow)
             playerScript_.setDirection(true);
         else
             playerScript_.stop();*/

        if (key == KeyCode.Space)
            Debug.Log("It's me, GAME");
        if (key == KeyCode.Return)
        {
            EventManager<string>.Raise(EnumEvent.LOADLEVEL, "StudMenu");
            stateManager_.changeState(StateEnum.MENU);
        }
    }
}
