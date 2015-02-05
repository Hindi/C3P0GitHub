using UnityEngine;
using System.Collections;

/// <summary>
/// This state is active when the player plays tetris.
/// It handles inputs and common UI.
/// </summary>

public class TetrisState : GameState {


    /// <summary>
    /// Int that represents the parameter chosen.
    /// </summary>
    private int paramId;


    /// <summary>
    /// Define the active camera effect at the beginning of a party.
    /// The effect depends of the parameter chosen.
    /// </summary>
    /// <param name="param"> The parameter the player chose</param>
    /// <returns> void </returns>
    public override void setParameter(Parameter param)
    {
        paramId = param.id;
        // Créer une caméra par effet, et on la met en main suivant le paramètre choisit
        // To do toujours util ?  remove
        GameObject.FindGameObjectWithTag("CameraManager").GetComponent<CameraManagerTetris>().setParamId(param.id);
    }



    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="stateManager"></param>
    public TetrisState(StateManager stateManager) : base(stateManager)
    {
        gameId = EnumGame.TETRIS;
    }

    /// <summary>
    /// Called when the lobby scen from Unity is loaded.
    /// </summary>
    /// <param name="lvl"> Id of the level loaded.</param>
    /// <returns>void</returns>
    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        ui.setParamCanvas(gameId);
        if (Application.isMobilePlatform)
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }

    /// <summary>
    /// Called when the game is over wether the player has lost, or won.
    /// </summary>
    /// <param name="b">Boolean that indicates if the player has won or not.</param>
    /// <returns>void</returns>
    public override void onGameOver(bool b)
    {
        if (loaded)
        {
            base.onGameOver(b);
        }
    }

    /// <summary>
    /// Called when restart.
    /// </summary>
    /// <returns>void</returns>
    public override void onGameRestart()
    {
        if (loaded)
        {
            base.onGameRestart();
            Grid._grid.gameRestart();
        }
    }



    /// <summary>
    /// Recieves all the necessary inputs (keyboard, gamepad and mouse).
    /// </summary>
    /// <param name="key">The input sent when it's a keyboard</param>
    /// <param name="inputs">The inputs sent when it's a touchscreen of a mobile for example.</param>
    /// <returns>void</returns>
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


    /// <summary>Recieves all the necessary inputs from a keyboard.</summary>
    /// <param name="key">The input sent.</param>
    /// <returns>void</returns>
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
    /// <summary>
    /// Called when leaving this state.
    /// </summary>
    /// <returns>void</returns>
    public override void end()
    {
        base.end();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.AutoRotation;
    }

}
