using UnityEngine;
using System.Collections;

/// <summary>
/// Class that contains all the RPC needed for Asteroid
/// </summary>
public class AsteroidNetwork : MonoBehaviour {

    // TO DO remove? public static NetworkViewID viewId = Network.AllocateViewID();

    private AsteroidsManager asteroidsManager;


    public void createAsteroid(Vector3 pos, Vector3 target, int hp, int id, int prefabId)
    {
        // TO DO remettre l'appel au rpc
        //networkView.RPC("createAsteroidRPC", RPCMode.All, pos, target, hp, id, prefabId);
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, prefabId);
    }




    public void createAsteroidColor(Vector3 pos, Vector3 target, int hp, int id, int prefabId, EnumColor color)
    {
        // TO DO remettre l'appel au rpc
        //networkView.RPC("createAsteroidColorRPC", RPCMode.All, pos, target, hp, id, prefabId, (int) color);
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id,prefabId, color);
    }

    public void destroyAsteroid(int id)
    {
        //networkView.RPC("destroyAsteroidRPC", RPCMode.All, id);
        asteroidsManager.remove(id);
    }

    public void hitAsteroid(int id)
    {
        // TO DO remettre l'appel au rpc
        //networkView.RPC("hitAsteroidRPC", RPCMode.Server, id);
        asteroidsManager.hit(id);
    }

    [RPC]
    private void hitAsteroidRPC(int id)
    {
        asteroidsManager.hit(id);
    }

    [RPC]
    private void destroyAsteroidRPC(int id)
    {
        asteroidsManager.remove(id);
    }

    [RPC]
    private void createAsteroidRPC(Vector3 pos, Vector3 target, int hp, int id, int prefabId)
    {
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, prefabId);
    }

    [RPC]
    private void createAsteroidColorRPC(Vector3 pos, Vector3 target, int hp, int id, int prefabId, int color)
    {
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, prefabId, (EnumColor)color);
    }
   

	// Use this for initialization
	void Start () {
        asteroidsManager = GameObject.FindGameObjectWithTag("asteroidManager").GetComponent<AsteroidsManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}


}
