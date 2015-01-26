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

	// Use this for initialization
	void Start () {
        kalman = new Kalman(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0), 
            0.01, 1);
	}
	
	// Update is called once per frame
	void Update () {
        kalman.addObservation(new Vector4(player.transform.position.x, 0, player.transform.position.y, 0));
        target.transform.position = kalman.PosInterp;//kalman.interpolation(60);
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
            0.01, 1);
    }
}
