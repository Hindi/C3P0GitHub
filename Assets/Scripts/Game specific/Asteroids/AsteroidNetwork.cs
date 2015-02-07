using UnityEngine;
using System.Collections;

/// <summary>
/// Class that contains all the RPC needed for Asteroid
/// </summary>
public class AsteroidNetwork : MonoBehaviour {

    // TO DO remove? public static NetworkViewID viewId = Network.AllocateViewID();

    private AsteroidsManager asteroidsManager;

   
    // TO DO il faudra peut être rajouter la vitesse de l'astéroid

    public void createAsteroid(Vector3 pos, Vector3 target, int hp, int id)
    {
        // TO DO remettre l'appel au rpc
        //networkView.RPC("createAsteroidRPC", RPCMode.All, pos, target, hp, id);
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, EnumColor.NONE);
    }


    [RPC]
    private void createAsteroidRPC(Vector3 pos, Vector3 target, int hp, int id)
    {
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, EnumColor.NONE);
    }


    public void createAsteroidColor(Vector3 pos, Vector3 target, int hp, int id, EnumColor color)
    {
        // TO DO remettre l'appel au rpc
        //networkView.RPC("createAsteroidColorRPC", RPCMode.All, pos, target, hp, id, (int) color);
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, color);
    }



    [RPC]
    private void createAsteroidColorRPC(Vector3 pos, Vector3 target, int hp, int id, int color)
    {
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, (EnumColor) color);
    }


    public void hitAsteroid(int id)
    {
        // TO DO remettre l'appel au rpc
        //networkView.RPC("hitAsteroidRPC", RPCMode.All, id);
        asteroidsManager.hit(id);
    }

    [RPC]
    private void hitAsteroidRPC(int id)
    {
        asteroidsManager.hit(id);
    }


   

	// Use this for initialization
	void Start () {
        asteroidsManager = GameObject.FindGameObjectWithTag("asteroidManager").GetComponent<AsteroidsManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}


}
