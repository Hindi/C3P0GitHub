using UnityEngine;
using System.Collections;

/// <summary>
/// Class that contains all the RPC needed for Asteroid
/// </summary>
public class AsteroidNetwork : MonoBehaviour {

    // TO DO remove? public static NetworkViewID viewId = Network.AllocateViewID();

    private AsteroidsManager asteroidsManager;

   
    public void createAsteroid(Vector3 pos, Vector3 target, int hp, int id)
    {
        networkView.RPC("createAsteroidRPC", RPCMode.All, pos, target, hp, id, EnumColor.NONE);
    }


    // 
    [RPC]
    private void createAsteroidRPC(Vector3 pos, Vector3 target, int hp, int id)
    {
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, EnumColor.NONE);
    }


    public void createAsteroidColor(Vector3 pos, Vector3 target, int hp, int id, EnumColor color)
    {
        networkView.RPC("createAsteroidColorRPC", RPCMode.All, pos, target, hp, id, color);
    }



    [RPC]
    private void createAsteroidColorRPC(Vector3 pos, Vector3 target, int hp, int id, EnumColor color)
    {
        AsteroidFactory._factory.createAsteroid(pos, target, hp, id, color);
    }


    public void hitAsteroid(int id)
    {
        networkView.RPC("hitAsteroidRPC", RPCMode.All, id);
    }

    [RPC]
    private void hitAsteroidRPC(int id)
    {
        asteroidsManager.hit(id);
    }


   

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}


}
