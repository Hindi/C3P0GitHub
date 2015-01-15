using UnityEngine;
using System.Collections;

public class PongState : GameState {

    private PongManagerScript gameScript;

    public PongState(StateManager stateManager)
        : base(stateManager)
    {
    }

    public override void onGameRestart()
    {
        base.onGameRestart();
    }

    public override void setParameter(Parameter param)
    {
        if (gameScript != null)
        {
            gameScript.setParameter(param);
        }
        else
        {
            Debug.Log("gameScript wasn't yet set when trying to set parameter");
        }
    }

    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        gameScript = GameObject.FindGameObjectWithTag("PongManagerScript").GetComponent<PongManagerScript>();
    }

    public override void end()
    {
        base.end();
    }

    public override void update()
    {
        base.update();
    }


}
