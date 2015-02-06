using UnityEngine;
using System.Collections;

public class EnemySpaceWar : Spaceship {

    [SerializeField]
    private GameObject player, playerProjectile;

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
        if(p != null && p.id == 1)
            kalman = new Kalman(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0), 
                1, 1);
        else
            kalman = new Kalman(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0),
                0.1, 0.1);
        posHistory = new Vector2[60];
        posHistoryIterator = 0;
        stateTimer = Time.time;
        IAState = (p != null && p.id == 0) ? SWIAState.RANDOM : SWIAState.DODGE_ATTACKS;
        actionNumber = 0;

        
//#if UNITY_EDITOR
//#else
        target.SetActive(false);
        noisedPosition.SetActive(false);
        imprecision.SetActive(false);
//#endif
	}
	
	// Update is called once per frame
    void Update()
    {
        base.Update();
        if(p != null && p.id != 0) kalman.addObservation(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0));
        addInterpretedPosition(kalman.PosInterp);

        if (isEnd)
        {
            if(Time.time - initEndTimer >= 2)
            {
                isEnd = false;
                EventManager<bool>.Raise(EnumEvent.SPACESHIPDESTROYED, false);
            }
        }

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
                        stateTimer = Time.time;
                        actionNumber = 0;
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
                        case SWIAState.DODGE_ATTACKS:
                            {
                                IAState = SWIAState.RANDOM;
                                moveRandomly();
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
                                    moveIntelligently();
                                }
                            } break;
                    }
                } break;
        }
	}

    public override void onHit()
    {
        base.onHit();
    }

    public void setParameter(Parameter param)
    {
        p = param;
    }

    public override void onRestart()
    {
        base.onRestart();
        if (p != null && p.id == 1)
            kalman = new Kalman(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0),
                1, 1);
        else
            kalman = new Kalman(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0),
                0.1, 0.1);
        posHistory = new Vector2[60];
        posHistoryIterator = 0;
        stateTimer = Time.time;
        IAState = (p != null && p.id == 0) ? SWIAState.RANDOM : SWIAState.DODGE_ATTACKS;
        IAAction = (p != null && p.id == 0) ? SWIAAction.MOVE_TURN_LEFT : SWIAAction.MOVE_FORWARD;
        actionNumber = 0;
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
        if ((int)((Time.time - stateTimer) / (stateChangeTimer / actionsPerTimer)) >= actionNumber) // time to decide a new action
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
        }


        switch (IAAction)
        {
            case SWIAAction.MOVE_FORWARD:
                {
                    goForward();
                } break;
            case SWIAAction.MOVE_TURN_LEFT:
                {
                    goForward();
                    rotate(1);
                } break;
            case SWIAAction.MOVE_TURN_RIGHT:
                {
                    goForward();
                    rotate(-1);
                } break;
            case SWIAAction.TURN_LEFT:
                {
                    rotate(1);
                } break;
            case SWIAAction.TURN_RIGHT:
                {
                    rotate(-1);
                } break;
            case SWIAAction.NOTHING:
                {
                    // nothing
                } break;
        }
    }

    private void moveIntelligently()
    {
        if ((int)((Time.time - stateTimer) / (stateChangeTimer / actionsPerTimer)) >= actionNumber) // time to decide a new action
        {
            actionNumber++;
            int i = Random.Range(1, 4);
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
            }
        }


        switch (IAAction)
        {
            case SWIAAction.MOVE_FORWARD:
                {
                    goForward();
                } break;
            case SWIAAction.MOVE_TURN_LEFT:
                {
                    goForward();
                    rotate(1);
                } break;
            case SWIAAction.MOVE_TURN_RIGHT:
                {
                    goForward();
                    rotate(-1);
                } break;
            case SWIAAction.TURN_LEFT:
                {
                    rotate(1);
                } break;
            case SWIAAction.TURN_RIGHT:
                {
                    rotate(-1);
                } break;
            case SWIAAction.NOTHING:
                {
                    // nothing
                } break;
        }
    }

    private void attackRandomly()
    {
        fire();
    }

    private bool dodgeAttacks()
    {
        bool spirale = dodgeSpirale();
        bool self = dodgeSelfProjectile();
        bool player = dodgePlayerProjectile();
        if (Time.time - stateTimer > stateChangeTimer / 2)
        {
            stateTimer = Time.time;
            IAState = SWIAState.MOVE_TOWARDS_PLAYER;
        }
        return (self && player && spirale);
    }

    private bool dodgeSpirale()
    {
        if (distance(spiral) <= 2)
        {
            float Angle = -transform.eulerAngles.z + Mathf.Atan2(transform.position.y - spiral.transform.position.y, transform.position.x - spiral.transform.position.x) * Mathf.Rad2Deg + 90;
            if (Angle >= 0)
            {
                rotate(-1);
                goForward();
            }
            else
            {
                rotate(1);
                goForward();
            }
            return false;
        }
        return true;
    }

    private bool dodgeSelfProjectile()
    {
        if (distance(playerProjectile) <= 2 && projectile.activeInHierarchy)
        {
            float Angle = -transform.eulerAngles.z + Mathf.Atan2(transform.position.y - projectile.transform.position.y, transform.position.x - projectile.transform.position.x) * Mathf.Rad2Deg + 90;
            if (Angle >= 0)
            {
                rotate(-1);
                goForward();
            }
            else
            {
                rotate(1);
                goForward();
            }
            return false;
        }
        return true;
    }

    private bool dodgePlayerProjectile()
    {
        if (distance(playerProjectile) <= 2 && playerProjectile.activeInHierarchy)
        {
            float Angle = - transform.eulerAngles.z + Mathf.Atan2(transform.position.y - playerProjectile.transform.position.y, transform.position.x - playerProjectile.transform.position.x) * Mathf.Rad2Deg + 90;
            if (Angle >= 0 || Angle <= -180)
            {
                rotate(-1);
                goForward();
            }
            else
            {
                rotate(1);
                goForward();
            }
            return false;
        }
        return true;
    }

    private float distance(GameObject g)
    {
        return (Mathf.Sqrt(Mathf.Pow(transform.position.x - g.transform.position.x,2) + Mathf.Pow(transform.position.y - g.transform.position.y,2)));
    }

    private void moveTowardsPlayer()
    {
        float Angle = -transform.eulerAngles.z + Mathf.Atan2(transform.position.y - player.transform.position.y, transform.position.x - player.transform.position.x) * Mathf.Rad2Deg + 90;
        if (Angle >= 0 || Angle <= -180)
        {
            rotate(1);
            goForward();
        }
        else
        {
            rotate(-1);
            goForward();
        }
        if (distance(player) <= 3)
        {
            stateTimer = Time.time;
            IAState = SWIAState.ATTACK_PLAYER;
        }
    }

    private void attackPlayer()
    {
        Vector2 prediction = interpolation(60);
        float Angle = -transform.eulerAngles.z + Mathf.Atan2(transform.position.y - prediction.y, transform.position.x - prediction.x) * Mathf.Rad2Deg + 90;
        if (Angle >= 0 || Angle <= -180)
        {
            rotate(1);
        }
        else
        {
            rotate(-1);
        }
        if (Mathf.Abs(Angle) <= 1)
        {
            fire();
            stateTimer = Time.time;
            IAState = SWIAState.DODGE_ATTACKS;
        }
        if (Time.time - stateTimer > stateChangeTimer)
        {
            stateTimer = Time.time;
            IAState = SWIAState.DODGE_ATTACKS;
        }
    }

}
