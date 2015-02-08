using UnityEngine;
using System.Collections;

public class EnemySpaceWar : Spaceship {
    /// <summary>
    /// Player and player's projectile GameObjects
    /// </summary>
    [SerializeField]
    private GameObject player, playerProjectile;

    /// <summary>
    /// GameObject used to graphically show the Kalman
    /// </summary>
    [SerializeField]
    private GameObject target;

    /// <summary>
    /// GameObject used to graphically show the Kalman
    /// </summary>
    [SerializeField]
    private GameObject noisedPosition;

    /// <summary>
    /// GameObject used to graphically show the Kalman
    /// </summary>
    [SerializeField]
    private GameObject imprecision;

    /// <summary>
    /// Kalman filter to provide a realistic observation of the player's position.
    /// </summary>
    private Kalman kalman;
    private Parameter p;

    /// <summary>
    /// History of positions that went through the Kalman filter.
    /// </summary>
    /// <remarks>When instanciated, size MUST be an even number</remarks>
    private Vector2[] posHistory; // Requiers un nombre pair de valeurs! Très Important!
    /// <summary>
    /// Used to write to the correct cell of the Array
    /// </summary>
    private int posHistoryIterator;

    /// <summary>
    /// Timer used to determine when to change AI's state
    /// </summary>
    private float stateTimer;
    /// <summary>
    /// Fixed value indicating when stateTimer should trigger
    /// </summary>
    [SerializeField]
    private float stateChangeTimer;
    /// <summary>
    /// Fixed value indicating how many times the AI changes its action during a state
    /// </summary>
    [SerializeField]
    private int actionsPerTimer;
    /// <summary>
    /// The number of actions already started during this state cycle
    /// </summary>
    private int actionNumber;
    /// <summary>
    /// The current state of the AI
    /// </summary>
    private SWIAState IAState;
    /// <summary>
    /// The actual movement (= action) the AI is currently doing
    /// </summary>
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

    void OnTriggerEnter2D(Collider2D collider)
    {
        try
        {
            collider.GetComponent<PlayerSpaceWar>().onHit();
        }
        catch (System.NullReferenceException)
        {

        }
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
                EventManager<bool, int>.Raise(EnumEvent.SPACESHIPDESTROYED, false, 5 * scoreMult);
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

    /// <summary>
    /// Called when the ship is hit by a projectile or the center spiral
    /// </summary>
    public override void onHit()
    {
        base.onHit();
    }

    /// <summary>
    /// Sets the parameter dictating the AI's state machine
    /// </summary>
    /// <param name="param">The new AI parameter</param>
    public void setParameter(Parameter param)
    {
        p = param;
        onRestart();
    }

    /// <summary>
    /// Resets the AI to be ready for a new round
    /// </summary>
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
    /// <summary>
    /// Adds a new position estimated by the Kalman to the history
    /// </summary>
    /// <param name="position">Position estimated by Kalman filter</param>
    private void addInterpretedPosition(Vector2 position)
    {
        posHistory[posHistoryIterator] = new Vector2(position.x, position.y);
        posHistoryIterator = (posHistoryIterator == posHistory.Length - 1) ? 0 : posHistoryIterator + 1;
    }

    /// <summary>
    /// Provides a prediction for the player's position using estimation history.<br/>
    /// Much more reliable than the Kalman's own interpolation due to the player's overall speed being lower than the noise speed
    /// </summary>
    /// <param name="nbFrames">The number of time units in which we want to know the player's position</param>
    /// <returns>A position vector</returns>
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
    /// <summary>
    /// Change the State when parameter is set to Random
    /// </summary>
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

    /// <summary>
    /// Determines an action and executes it when State is set to Move Randomly
    /// </summary>
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

    /// <summary>
    /// Determines an action and executes it when state is set to Dodge Attacks and there's nothing to dodge
    /// </summary>
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

    /// <summary>
    /// Makes the AI fire at will
    /// </summary>
    private void attackRandomly()
    {
        fire();
    }

    /// <summary>
    /// Tries pretty badly to adjust the trajectory to dodge projectiles as well as the center spiral
    /// </summary>
    /// <returns>True if there's nothing to dodge, false otherwise</returns>
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

    /// <summary>
    /// Tries to adjust the trajectory to dodge the center spiral
    /// </summary>
    /// <returns>False if the trajectory has to be altered, True otherwise</returns>
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

    /// <summary>
    /// Tries to adjust the trajectory to dodge self's projectile
    /// </summary>
    /// <returns>False if the trajectory has to be altered, True otherwise</returns>
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

    /// <summary>
    /// Tries to adjust the trajectory to dodge player's projectile
    /// </summary>
    /// <returns>False if the trajectory has to be altered, True otherwise</returns>
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

    /// <summary>
    /// Similar to Vector2.Distance, but with this AI's position
    /// </summary>
    /// <param name="g">GameObject to determine the distance with</param>
    /// <returns>Distance between this and g</returns>
    private float distance(GameObject g)
    {
        return (Mathf.Sqrt(Mathf.Pow(transform.position.x - g.transform.position.x,2) + Mathf.Pow(transform.position.y - g.transform.position.y,2)));
    }

    /// <summary>
    /// Determine an action and executes it to move towards the player, regardless of any obstacles
    /// </summary>
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

    /// <summary>
    /// Aims at player's interpolated position in 60 frames and shoots if facing it
    /// </summary>
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
