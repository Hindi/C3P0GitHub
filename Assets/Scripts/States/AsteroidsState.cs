using UnityEngine;
using System.Collections;

public class AsteroidsState : GameState
{
    private int paramId;


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
        ui.onCloseMenu();
        //ui.setParamCanvas(gameId);

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
           /* foreach (var t in inputs)
            {
                if (t.phase == TouchPhase.Began)
                {
                    if (t.position.x > 2 * Screen.width / 3)
                        playerScript_.setDirection(true);
                    else if (t.position.x < Screen.width / 3)
                        playerScript_.setDirection(false);
                    else
                        playerScript_.fire();
                }
                if (t.phase == TouchPhase.Ended)
                {
                    if (t.position.x > 2 * Screen.width / 3 || t.position.x < Screen.width / 3)
                        playerScript_.stop();
                }
            }*/
        }
    }

    public override void noticeInput(EnumInput key)
    {
        if (loaded)
        {
            /*base.noticeInput(key);
            if (key == EnumInput.SPACE)
                playerScript_.fire();
            if (key == EnumInput.LEFT)
                playerScript_.move(-1);
            else if (key == EnumInput.RIGHT)
                playerScript_.move(1);
            if (key == EnumInput.LEFTUP || key == EnumInput.RIGHTUP)
                playerScript_.stop();*/
        }
    }
}
