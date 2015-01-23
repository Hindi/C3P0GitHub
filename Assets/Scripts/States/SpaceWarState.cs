using UnityEngine;
using System.Collections;

public class SpaceWarState : GameState {

    private SpaceWarScript gameScript;
    private Parameter p;

	public SpaceWarState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.PONG;
    }

    public override void onGameRestart()
    {
        if (loaded)
        {
            base.onGameRestart();
            gameScript.onRestart();
        }
    }

    public override void setParameter(Parameter param)
    {
        if (gameScript != null)
        {
            gameScript.setParameter(param);
            p = param;
        }
        else
        {
            Debug.Log("gameScript wasn't yet set when trying to set parameter");
        }
    }


    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        gameScript = GameObject.FindGameObjectWithTag("SpaceWarScript").GetComponent<SpaceWarScript>();
        ui.setParamCanvas(gameId);
    }

    public override void end()
    {
        base.end();
    }

    public override void update()
    {
        base.update();
    }

    public override void onGameOver(bool b)
    {
        if (loaded)
        {
            base.onGameOver(b);
            /* TODO : compléter avec le score réel 
            C3PONetworkManager.Instance.sendGameStats((int)gameId, p.id, gameScript.playerScore - gameScript.enemyScore);
             * */
        }
    }

}
