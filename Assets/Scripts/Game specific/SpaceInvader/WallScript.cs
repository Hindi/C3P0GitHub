using UnityEngine;
using System.Collections;

/// <summary>Wall.</summary>
public class WallScript : MonoBehaviour {

    /// <summary>Reference to the alien manager.</summary>
    [SerializeField]
    private AliensManager alienManager;

    /// <summary>Reverse aliens dirrection when one of them collide.</summary>
    void OnTriggerEnter(Collider other)
    {
		if (other.tag == "Enemy") 
		{
			alienManager.reverseDirection ();
		}
    }
}
