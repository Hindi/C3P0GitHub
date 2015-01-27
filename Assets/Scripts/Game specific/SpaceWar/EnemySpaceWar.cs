using UnityEngine;
using System.Collections;

public class EnemySpaceWar : Spaceship {

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject target;

    [SerializeField]
    private GameObject noisedPosition;

    [SerializeField]
    private GameObject imprecision;

    private Kalman kalman;
    private Parameter p;

    private Vector2[] posHistory; // Requiers un nombre pair de valeurs! Très Important!
    private int posHistoryIterator;

    private float stateTimer;
    [SerializeField]
    private float stateChangeTimer;
    [SerializeField]
    private int actionsPerTimer;
    private int actionNumber;
    private SWIAState IAState;
    private SWIAAction IAAction;

    /*************************************************************************************************
     * Public functions & standard Unity callbacks                                                   *
     *************************************************************************************************/

	// Use this for initialization
	void Start () {
        kalman = new Kalman(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0), 
            0.1, 0.1);
        posHistory = new Vector2[60];
        posHistoryIterator = 0;

        
#if UNITY_EDITOR
#else
        target.SetActive(false);
        noisedPosition.SetActive(false);
        imprecision.SetActive(false);
#endif
	}
	
	// Update is called once per frame
	void Update () {
        kalman.addObservation(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0));
        addInterpretedPosition(kalman.PosInterp);

#if UNITY_EDITOR
        /* DEBUG : Moving points around */
        //target.transform.position = kalman.interpolation(60);
        //target.transform.position = kalman.PosInterp;
        target.transform.position = interpolation(60);
        noisedPosition.transform.position = kalman.PosBruit;
        imprecision.transform.position = kalman.PosImprecise;
#endif
        if(p == null) return;
        // IA State management
        switch (p.id)
        {
            case 0: // IA Aléatoire
                {
                    if (Time.time - stateTimer > stateChangeTimer)
                    {
                        changeRandomState();
                    }
                    switch (IAState)
                    {
                        case SWIAState.RANDOM:
                            {
                                moveRandomly();
                                attackRandomly();
                            } break;
                        case SWIAState.MOVE_RANDOMLY:
                            {
                                moveRandomly();
                            } break;
                        case SWIAState.ATTACK_RANDOMLY:
                            {
                                attackRandomly();
                            } break;
                    }
                } break;

            case 1:
            case 2:
                {
                    switch (IAState)
                    {
                        case SWIAState.MOVE_TOWARDS_PLAYER:
                            {
                                if (dodgeAttacks())
                                {
                                    moveTowardsPlayer();
                                }
                            } break;
                        case SWIAState.ATTACK_PLAYER:
                            {
                                attackPlayer();
                            } break;
                        case SWIAState.DODGE_ATTACKS:
                            {
                                if (dodgeAttacks())
                                {
                                    moveRandomly();
                                }
                            } break;
                    }
                } break;
        }
	}

    public override void onHit()
    {
        base.onHit();
        EventManager<bool>.Raise(EnumEvent.SPACESHIPDESTROYED, false);
    }

    public void setParameter(Parameter param)
    {
        p = param;
    }

    public void onRestart()
    {
        kalman = new Kalman(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0), 
            0.1, 0.1);
        posHistory = new Vector2[60];
        posHistoryIterator = 0;
    }

    /*************************************************************************************************
     * Private used functions                                                                        *
     *************************************************************************************************/

    private void addInterpretedPosition(Vector2 position)
    {
        posHistory[posHistoryIterator] = new Vector2(position.x, position.y);
        posHistoryIterator = (posHistoryIterator == posHistory.Length - 1) ? 0 : posHistoryIterator + 1;
    }

    private Vector2 interpolation(int nbFrames)
    {
        Vector2 oldPos = new Vector2(0, 0), newPos = new Vector2(0, 0), speed5f;
        for (int i = 0; i < posHistory.Length / 2; i++) // première moitié
        {
            oldPos.x += posHistory[posHistoryIterator].x;
            oldPos.y += posHistory[posHistoryIterator].y;
            posHistoryIterator = (posHistoryIterator == posHistory.Length - 1) ? 0 : posHistoryIterator + 1;
        }
        oldPos.x /= posHistory.Length / 2;
        oldPos.y /= posHistory.Length / 2;
        for (int i = 0; i < posHistory.Length / 2; i++) // deuxième moitié
        {
            newPos.x += posHistory[posHistoryIterator].x;
            newPos.y += posHistory[posHistoryIterator].y;
            posHistoryIterator = (posHistoryIterator == posHistory.Length - 1) ? 0 : posHistoryIterator + 1;
        }
        newPos.x /= posHistory.Length / 2;
        newPos.y /= posHistory.Length / 2;

        speed5f = new Vector2(newPos.x - oldPos.x, newPos.y - oldPos.y);

        Vector2 result = new Vector2(kalman.PosInterp.x + (int)(nbFrames / (posHistory.Length / 2)) * speed5f.x, kalman.PosInterp.y + (int)(nbFrames / (posHistory.Length / 2)) * speed5f.y);
        return result;
    }


    /*************************************************************************************************
     * Private IA action functions                                                                   *
     *************************************************************************************************/

    private void changeRandomState()
    {
        int i = Random.Range(1, 4);
        switch (i)
        {
            case 1:
                {
                    IAState = SWIAState.RANDOM;
                } break;
            case 2:
                {
                    IAState = SWIAState.MOVE_RANDOMLY;
                } break;
            case 3:
                {
                    IAState = SWIAState.ATTACK_RANDOMLY;
                } break;
        }
    }
    private void moveRandomly()
    {
        if ((int)((Time.time - stateTimer) / (stateChangeTimer / actionsPerTimer)) == actionNumber) // time to decide a new action
        {
            actionNumber++;
            int i = Random.Range(1, 7);
            switch (i)
            {
                case 1:
                    {
                        IAAction = SWIAAction.MOVE_FORWARD;
                    } break;
                case 2:
                    {
                        IAAction = SWIAAction.MOVE_TURN_LEFT;
                    } break;
                case 3:
                    {
                        IAAction = SWIAAction.MOVE_TURN_RIGHT;
                    } break;
                case 4:
                    {
                        IAAction = SWIAAction.TURN_RIGHT;
                    } break;
                case 5:
                    {
                        IAAction = SWIAAction.TURN_RIGHT;
                    } break;
                case 6:
                    {
                        IAAction = SWIAAction.NOTHING;
                    } break;
            }
            Debug.Log("New action : " + IAAction);
        }


        switch (IAAction)
        {
            case SWIAAction.MOVE_FORWARD:
                {
                    goForward();
                    Debug.Log("Go forward");
                } break;
            case SWIAAction.MOVE_TURN_LEFT:
                {
                    goForward();
                    Debug.Log("Go forward");
                    rotate(1);
                    Debug.Log("Turn left");
                } break;
            case SWIAAction.MOVE_TURN_RIGHT:
                {
                    goForward();
                    Debug.Log("Go forward");
                    rotate(-1);
                    Debug.Log("Turn right");
                } break;
            case SWIAAction.TURN_LEFT:
                {
                    rotate(1);
                    Debug.Log("Turn left");
                } break;
            case SWIAAction.TURN_RIGHT:
                {
                    rotate(-1);
                    Debug.Log("Turn right");
                } break;
            case SWIAAction.NOTHING:
                {
                    // nothing
                } break;
        }
    }
    private void attackRandomly()
    {

    }
    private bool dodgeAttacks()
    {
        return true;
    }
    private void moveTowardsPlayer()
    {

    }
    private void attackPlayer()
    {

    }

}
