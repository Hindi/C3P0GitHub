using UnityEngine;
using System.Collections;

public class ExplosionLifeTime : MonoBehaviour {

    /// <summary>
    /// Fixed value for duration in which the projectile stays alive once fired
    /// </summary>
    [SerializeField]
    private float lifeTime;

	// Use this for initialization
	void Start () {
        Destroy(gameObject,lifeTime);
	}
	
	// Update is called once per frame
	void Update () {

	}
}
