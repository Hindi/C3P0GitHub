using UnityEngine;
using System.Collections;

public class Asteroid : MonoBehaviour {

    private AsteroidShip shipScript;
    private Vector3 target;
    public int hp;

    [SerializeField]
    private float speed;

	// Use this for initialization
	void Start () {
        shipScript = GameObject.FindGameObjectWithTag("Ship").GetComponent<AsteroidShip>();
        target = shipScript.getTarget();
        hp = Random.Range(1, 5);
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
	}

    public void hit()
    {
        hp--;
        // TO DO rpc pour mettre à jour les HP pour tout le monde
        if (hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

}
