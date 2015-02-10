using UnityEngine;
using System.Collections;

public class AsteroidsState : GameState
{
    private PlayerAsteroid playerScript_;

    private GameObject player_;

    public AsteroidsState(StateManager stateManager)
        : base(stateManager)
    {
        gameId = EnumGame.ASTEROIDS;
    }


    public override void onGameRestart()
    {
        if (loaded)
        {
            base.onGameRestart();
        }
    }

    public override void onGameOver(bool b)
    {
        if (loaded)
        {
            base.onGameOver(b);

        }
    }


    public override void setParameter(Parameter param)
    {
        paramId = param.id;

    }

    public override void onLevelWasLoaded(int lvl)
    {
        base.onLevelWasLoaded(lvl);
        loaded = true;
        player_ = GameObject.FindGameObjectWithTag("Player");
        playerScript_ = player_.GetComponent<PlayerAsteroid>();
        GameObject.FindGameObjectWithTag("UI").GetComponent<UI>().closeMenus();
        applyPause(false);
        ui.onCloseMenu();
        //ui.setParamCanvas(gameId);
        //playerScript_.updateElementsResolution();
        setParameter(new Parameter());
    }

    // Use this for initialization
    public override void start()
    {
        base.start();
    }


    // Use this for state transition
    public override void end()
    {
        base.end();
        if (Application.isMobilePlatform)
            Screen.orientation = ScreenOrientation.AutoRotation;
    }

    // Update is called once per frame
    public override void update()
    {
        base.update();
    }

    public override void noticeInput(EnumInput key, Touch[] inputs)
    {
        if (loaded)
        {
            foreach (var t in inputs)
            {
                if (t.phase == TouchPhase.Began || t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
                {
                    if (t.position.x > 2 * Screen.width / 3)
                        playerScript_.moveRight(true);
                    else if (t.position.x < Screen.width / 3)
                        playerScript_.moveRight(false);
                    if (t.position.y > 2 * Screen.height / 3)
                        playerScript_.moveUp(true);
                    else if (t.position.y < Screen.height / 3)
                        playerScript_.moveUp(false);

                    // if we touch the center part of the screen, we fire
                    // fire handles cooldown
                    if (t.position.x < 2 * Screen.width / 3 &&
                        t.position.x > Screen.width / 3 &&
                        t.position.y < 2 * Screen.height / 3 &&
                        t.position.y > Screen.height / 3)
                        playerScript_.fire();
                }
            }
        }
    }

    public override void noticeInput(EnumInput key)
    {
        if (loaded)
        {
            //base.noticeInput(key);
            if (key == EnumInput.SPACE)
                playerScript_.fire();
            if (key == EnumInput.LEFT)
                playerScript_.rotLeftRight -= playerScript_.speed;
            else if (key == EnumInput.RIGHT)
                playerScript_.rotLeftRight += playerScript_.speed;
            if (key == EnumInput.UP)
                playerScript_.rotUpDown -= playerScript_.speed;
            else if (key == EnumInput.DOWN)
                playerScript_.rotUpDown += playerScript_.speed;
        }
    }


}
