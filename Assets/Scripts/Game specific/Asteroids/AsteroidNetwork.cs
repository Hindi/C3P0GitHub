using UnityEngine;
using System.Collections;

/// <summary>
/// Class that contains all the RPC needed for Asteroid
/// </summary>
public class AsteroidNetwork : MonoBehaviour {

    // TO DO remove? public static NetworkViewID viewId = Network.AllocateViewID();

    private AsteroidsManager asteroidsManager;

    // TO DO rajouter un argument pour donner une couleur ou non à l'astéroid (accéder au Halo)
    [RPC]
    public void createAsteroid(Vector3 pos, Vector3 target, int hp, bool b)
    {
        AsteroidFactory._factory.createAsteroid(pos, target, hp, b);
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
