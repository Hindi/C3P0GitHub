using UnityEngine;
using System.Collections;

/// <summary>
/// Class that contains all the RPC needed for Asteroid
/// </summary>
public class AsteroidNetwork : MonoBehaviour {

    public static NetworkViewID viewId = Network.AllocateViewID();

    private AsteroidsManager asteroidsManager;

    [RPC]
    public void createAsteroid(Vector3 pos, Vector3 target, int hp)
    {
        AsteroidFactory._factory.createAsteroid(pos, target, hp);
    }


    [RPC]
    public void hitAsteroid(int id)
    {
        asteroidsManager.hit(id);
    }


    [RPC]
    public void push(Asteroid ast)
    {
        AsteroidFactory._factory.push(ast);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}


}
