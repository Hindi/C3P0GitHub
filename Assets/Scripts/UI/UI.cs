using UnityEngine;
using System;
using System.Linq;
using System.Collections;

/// <summary>The main UI script.</summary>
public class UI : MonoBehaviour {

    /// <summary>Reference to the pause canvas.</summary>
	[SerializeField]
    private Canvas pauseMenu;

    /// <summary>Reference to the question canvas.</summary>
    [SerializeField]
    private Canvas questionMenu;
    public Canvas QuestionCanvas
    {
        get { return questionMenu; }
        set { questionMenu = value; }
    }

    /// <summary>Reference to the space invader params canvas.</summary>
    [SerializeField]
    private Canvas spaceInvaderParams;
    /// <summary>Reference to the tetris params canvas.</summary>
    [SerializeField]
    private Canvas tetrisParams;
    /// <summary>Reference to the pong params canvas.</summary>
    [SerializeField]
    private Canvas pongParams;
    /// <summary>Reference to the spacewar params canvas.</summary>
    [SerializeField]
    private Canvas spaceWarParams;
    /// <summary>Reference to the lunar lander params canvas.</summary>
    [SerializeField]
    private Canvas lunarLanderParams;
    /// <summary>Reference to the asteroids params canvas.</summary>
    [SerializeField]
    private Canvas asteroidsParam;
    /// <summary>Reference to the mario params canvas.</summary>
    [SerializeField]
    private Canvas marioParam;
    /// <summary>Reference to the pacman params canvas.</summary>
	[SerializeField]
    private Canvas pacmanParam;
    /// <summary>Reference to the default params canvas.</summary>
    [SerializeField]
    private Canvas paramMenu;
    /// <summary>Reference to the game over canvas.</summary>
    [SerializeField]
    private Canvas gameOverMenu;
    /// <summary>Reference to the connection canvas.</summary>
    [SerializeField]
    private Canvas connectionMenu;
    /// <summary>Reference to the connection prompt canvas.</summary>
    [SerializeField]
    private Canvas connectionPromptMenu;
    /// <summary>Reference to the server canvas.</summary>
    [SerializeField]
    private Canvas serverMenu;

    /// <summary>Reference to the current canvas.</summary>
    private Canvas currentCanvas;

    /// <summary>Add listeners for the ui related events.</summary>
    /// <returns>void</returns>
	void Start () {
        EventManager<bool>.AddListener(EnumEvent.PAUSEGAME, onGamePaused);
        EventManager.AddListener(EnumEvent.CLOSEMENU, onCloseMenu);
        EventManager <float>.AddListener(EnumEvent.CHANGEPARAM, onChangeParam);
        EventManager<bool>.AddListener(EnumEvent.GAMEOVER, onGameOver);
        EventManager.AddListener(EnumEvent.CONNECTIONSTATE, onConnectionState);
        EventManager.AddListener(EnumEvent.SERVERUI, onServerStart);
	}

    /// <summary>Callback called to close all the menus.</summary>
    /// <returns>void</returns>
    public void onCloseMenu()
    {
        closeMenus();
    }

    /// <summary>Called on connection.</summary>
    /// <returns>void</returns>
    public void onConnectionState()
    {
        updateCurrentCanvas(connectionMenu);
    }

    /// <summary>Display the correct parameter canvas.</summary>
    /// <param name="gameId">Enum of the current game.</param>
    /// <returns>void</returns>
    public void setParamCanvas(EnumGame gameId)
    {
        closeMenus();
        switch (gameId)
        {
            case EnumGame.SPACEINVADER:
                paramMenu = spaceInvaderParams;
                break;
            case EnumGame.TETRIS:
                paramMenu = tetrisParams;
                break;
            case EnumGame.LUNARLANDER:
                paramMenu = lunarLanderParams;
                break;
            case EnumGame.PONG:
                paramMenu = pongParams;
                break;
            case EnumGame.SPACEWAR:
                paramMenu = spaceWarParams;
                break;
            case EnumGame.ASTEROIDS:
                paramMenu = asteroidsParam;
                break;
            case EnumGame.MARIO:
                paramMenu = marioParam;
                break;
		case EnumGame.PACMAN:
			paramMenu = pacmanParam;
			break;
        }
        updateCurrentCanvas(paramMenu);
    }

    /// <summary>Clean a string (keep letters and digits.</summary>
    /// <param name="s">The string to be cleaned.</param>
    /// <returns>string : The cleaned string</returns>
    public static string cleanString(string s)
    {
        return new String(s.Where(Char.IsLetterOrDigit).ToArray());
    }

    /// <summary>Go back to the main menu (state & unity scene).</summary>
    /// <returns>void</returns>
    public void goToMainMenu()
    {
        EventManager<string>.Raise(EnumEvent.LOADLEVEL, "Connection");
    }

    /// <summary>Getter for the current canvas.</summary>
    /// <returns>Canvas : the current canvas</returns>
    public Canvas getcurrentCanvas()
    {
        return currentCanvas;
    }

    /// <summary>Called on game over. Dispay the game over canvas</summary>
    /// <param name="b">True if win.</param>
    /// <returns>void</returns>
    public void onGameOver(bool b)
    {
        updateCurrentCanvas(gameOverMenu);
    }

    /// <summary>Called on pause. Dispay or hide the pause canvas</summary>
    /// <param name="b">True to display.</param>
    /// <returns>void</returns>
	public void onGamePaused(bool b)
	{
        if (b)
            updateCurrentCanvas(pauseMenu);
        else
            closeMenus();
	}

    /// <summary>Called on change parameter. Dispay parameter canvas</summary>
    /// <param name="ratio">Float : the good answer ratio.</param>
    /// <returns>void</returns>
    public void onChangeParam(float ratio)
    {
        updateCurrentCanvas(paramMenu);
    }

    /// <summary>Called on server start. Dispay  the server canvas</summary>
    /// <param name="b">True to display.</param>
    /// <returns>void</returns>
    public void onServerStart()
    {
        updateCurrentCanvas(serverMenu);
    }

    public void switchToParam()
    {
        updateCurrentCanvas(paramMenu);
    }

    public void updateCurrentCanvas(Canvas newCanvas)
    {
        if (currentCanvas)
            currentCanvas.gameObject.SetActive(false);
        currentCanvas = newCanvas;
        currentCanvas.gameObject.SetActive(true);
    }

    public void closeMenus()
    {
        currentCanvas.gameObject.SetActive(false);
        paramMenu.gameObject.SetActive(false);
        if(!C3PONetwork.Instance.IS_SERVER)
        {
            gameOverMenu.gameObject.SetActive(false);
            pauseMenu.gameObject.SetActive(false);
            connectionMenu.gameObject.SetActive(false);
            connectionPromptMenu.gameObject.SetActive(false);
            questionMenu.gameObject.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
