using UnityEngine;
using System;
using System.Linq;
using System.Collections;

public class UI : MonoBehaviour {

	[SerializeField]
    private Canvas pauseMenu;
    [SerializeField]
    private Canvas questionMenu;
    public Canvas QuestionCanvas
    {
        get { return questionMenu; }
        set { questionMenu = value; }
    }

    [SerializeField]
    private Canvas scoreMenu;
    public Canvas ScoreMenu
    {
        get { return scoreMenu; }
        set { scoreMenu = value; }
    }

    [SerializeField]
    private Canvas spaceInvaderParams;
    [SerializeField]
    private Canvas tetrisParams;
    [SerializeField]
    private Canvas pongParams;
    [SerializeField]
    private Canvas spaceWarParams;
    [SerializeField]
    private Canvas lunarLanderParams;
    [SerializeField]
    private Canvas asteroidsParam;
    [SerializeField]
    private Canvas marioParam;
	[SerializeField]
	private Canvas pacmanParam;
    [SerializeField]
    private Canvas paramMenu;
    [SerializeField]
    private Canvas gameOverMenu;
    [SerializeField]
    private Canvas connectionMenu;
    [SerializeField]
    private Canvas connectionPromptMenu;
    [SerializeField]
    private Canvas serverMenu;

    private Canvas currentCanvas;

	// Use this for initialization
	void Start () {
        EventManager<bool>.AddListener(EnumEvent.PAUSEGAME, onGamePaused);
        EventManager.AddListener(EnumEvent.CLOSEMENU, onCloseMenu);
        EventManager <float>.AddListener(EnumEvent.CHANGEPARAM, onChangeParam);
        EventManager<bool>.AddListener(EnumEvent.GAMEOVER, onGameOver);
        EventManager.AddListener(EnumEvent.CONNECTIONSTATE, onConnectionState);
        EventManager.AddListener(EnumEvent.SERVERUI, onServerStart);
	}

    public void onCloseMenu()
    {
        closeMenus();
    }

    public void onConnectionState()
    {
        updateCurrentCanvas(connectionMenu);
    }

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

    public static string cleanString(string s)
    {
        return new String(s.Where(Char.IsLetterOrDigit).ToArray());
    }

    public void goToMainMenu()
    {
        EventManager<string>.Raise(EnumEvent.LOADLEVEL, "Connection");
    }

    public Canvas getcurrentCanvas()
    {
        return currentCanvas;
    }

    public void onGameOver(bool b)
    {
        updateCurrentCanvas(gameOverMenu);
    }

	public void onGamePaused(bool b)
	{
        if (b)
            updateCurrentCanvas(pauseMenu);
        else
            closeMenus();
	}

    public void onChangeParam(float ratio)
    {
        updateCurrentCanvas(paramMenu);
        paramMenu.GetComponent<ParamMenu>().activateButtons(ratio);
    }

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
