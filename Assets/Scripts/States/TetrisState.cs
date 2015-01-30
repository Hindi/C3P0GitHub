using UnityEngine;
using System.Collections;

public class TetrisState : GameState {

    private int paramId;

    public override void setParameter(Parameter param)
    {
        paramId = param.id;
        // Créer une caméra par effet, et on la met en main suivant le paramètre choisit
        // To do toujours util ?  remove
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
        if (Application.isMobilePlatform)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
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


    public override void noticeInput(EnumInput key, Touch[] inputs)
    {
        if (loaded)
        {
            foreach (var t in inputs)
            {
				if (t.phase == TouchPhase.Began && t.position.y > 2 * Screen.height / 3)
					Tetromino.fallingTetromino.rotate();
				else if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
				{
					if (t.position.y < Screen.height / 4)
						Tetromino.fallingTetromino.moveDown();
					else if (t.position.x > 2 * Screen.width / 3)
						Tetromino.fallingTetromino.moveRight();
					else if (t.position.x < Screen.width / 3)
						Tetromino.fallingTetromino.moveLeft();
				}
			
            }
        }
    }


    public override void noticeInput(EnumInput key)
    {
        
        if (loaded)
        {
            base.noticeInput(key);

            if (Tetromino.fallingTetromino != null)
            {
                switch (key)
                {
                    case EnumInput.RIGHT:
                        Tetromino.fallingTetromino.moveRight();
                        break;
                    case EnumInput.LEFT:
                        Tetromino.fallingTetromino.moveLeft();
                        break;
                    case EnumInput.UPDOWN:
                        Tetromino.fallingTetromino.rotate();
                        break;
                    case EnumInput.DOWN:
                        Tetromino.fallingTetromino.moveDown();
                        break;
                    default:
                        break;
                }
            }
        }
        
    }

    // Use this for state transition
    public override void end()
    {
        base.end();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.AutoRotation;
    }

}
