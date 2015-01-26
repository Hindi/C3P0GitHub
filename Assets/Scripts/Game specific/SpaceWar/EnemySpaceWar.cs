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
}
