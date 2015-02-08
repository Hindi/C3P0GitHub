using UnityEngine;
using System.Collections;

/// <summary>
/// Class that contains all the RPC needed for Asteroid
/// </summary>
public class AsteroidNetwork : MonoBehaviour {

    // TO DO remove? public static NetworkViewID viewId = Network.AllocateViewID();

    private AsteroidsManager asteroidsManager;


    /// <summary>
    /// Called by a player to ask his color
    /// </summary>
    /// <returns>void</returns>
    public void askInitPlayer()
    {
        networkView.RPC("askInitPlayerRPC", RPCMode.Server);
    }

    public void createAsteroid(Vector3 pos, Vector3 target, int hp, int id, int prefabId)
    {
        // TO DO remove test local
        networkView.RPC("createAsteroidRPC", RPCMode.All, pos, target, hp, id, prefabId);
        //AsteroidFactory._factory.createAsteroid(pos, target, hp, id, prefabId);
    }




    public void createAsteroidColor(Vector3 pos, Vector3 target, int hp, int id, int prefabId, EnumColor color)
    {
        // TO DO remove test local
        networkView.RPC("createAsteroidColorRPC", RPCMode.All, pos, target, hp, id, prefabId, (int) color);
        //AsteroidFactory._factory.createAsteroid(pos, target, hp, id,prefabId, color);
    }

    public void destroyAsteroid(int id)
    {
        // TO DO remove test local
        networkView.RPC("destroyAsteroidRPC", RPCMode.Others, id);
        //asteroidsManager.remove(id);
    }

    public void hitAsteroid(int id)
    {
        // TO DO remove test local
        networkView.RPC("hitAsteroidRPC", RPCMode.Server, id);
        //asteroidsManager.hit(id);
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


    /// <summary>
    /// Called by a player, only server do this function.
    /// </summary>
    /// <param name="info">A way to have the sender informations.</param>
    [RPC]
    public void askInitPlayerRPC(NetworkMessageInfo info)
    {
        //zone = asteroidsManager.choosePos();
        EnumColor col = asteroidsManager.chooseColor();
        //networkView.RPC("initPlayerRPC",info.sender,(int) col, zone);
    }

    [RPC]
    private void initPlayerRPC(int color, Vector3 zone)
    {
        asteroidsManager.initColor((EnumColor) color);
        //asteroidsManager.initZone(zone);
    }

	// Use this for initialization
	void Start () {
        asteroidsManager = GameObject.FindGameObjectWithTag("asteroidManager").GetComponent<AsteroidsManager>();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public bool isServer()
    {
        return Network.isServer;
    }

}
