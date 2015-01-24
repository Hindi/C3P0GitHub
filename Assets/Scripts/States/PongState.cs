using UnityEngine;
using System.Collections;

public class PongState : GameState {

    private PongManagerScript gameScript;
    private Parameter p;
    private PlayerControl player;

    public PongState(StateManager stateManager)
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
        gameScript = GameObject.FindGameObjectWithTag("PongManagerScript").GetComponent<PongManagerScript>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
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
            Debug.Log(gameScript.playerScore - gameScript.enemyScore);
            C3PONetworkManager.Instance.sendGameStats((int) gameId, p.id, gameScript.playerScore - gameScript.enemyScore);
        }
    }

    public override void noticeInput(EnumInput key)
    {
        Debug.Log(key);
        if (key == EnumInput.SPACE)
            gameScript.launchCoupSpecial();
        if(key == EnumInput.UP)
            player.goUp();
        if (key == EnumInput.DOWN)
            player.goDown();
    }

    public override void noticeInput(EnumInput key, Touch[] inputs)
    {

    }

}
