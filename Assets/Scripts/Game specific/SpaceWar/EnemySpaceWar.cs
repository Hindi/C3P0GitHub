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

    private Vector2[] posHistory;
    private int posHistoryIterator;

    /*************************************************************************************************
     * Public functions & standard Unity callbacks                                                   *
     *************************************************************************************************/

	// Use this for initialization
	void Start () {
        kalman = new Kalman(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0), 
            0.1, 0.01);
        posHistory = new Vector2[10];
        posHistoryIterator = 0;
	}
	
	// Update is called once per frame
	void Update () {
        kalman.addObservation(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0));
        //target.transform.position = kalman.interpolation(60);
        target.transform.position = kalman.PosInterp;
        noisedPosition.transform.position = kalman.PosBruit;
        imprecision.transform.position = kalman.PosImprecise;
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
            0.1, 0.01);
        posHistory = new Vector2[10];
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
}
