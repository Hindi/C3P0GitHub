using UnityEngine;
using System.Collections;

public class TetrisState : GameState {

    private int paramId;

    public override void setParameter(Parameter param)
    {
        paramId = param.id;
        // TO DO gérer effet 
        // Créer une caméra par effet, et on la met en main suivant le paramètre choisit
        GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManagerTetris>().setParamId(param.id);
    }

    public TetrisState(StateManager stateManager) : base(stateManager)
    {
        gameId = EnumGame.TETRIS;
    }
    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        ui.setParamCanvas(gameId);
    }

    public override void onGameOver(bool b)
    {
        if (loaded)
        {
            base.onGameOver(b);
            C3PONetworkManager.Instance.sendGameStats((int)gameId, paramId, Grid._grid.score);
        }
    }

    public override void onGameRestart()
    {
        if (loaded)
        {
            base.onGameRestart();
            Grid._grid.gameRestart();
        }
    }
}
