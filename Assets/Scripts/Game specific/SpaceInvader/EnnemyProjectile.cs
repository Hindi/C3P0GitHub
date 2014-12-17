using UnityEngine;
using System.Collections;

public class EnnemyProjectile : MonoBehaviour
{

    [SerializeField]
    int speed;

    [SerializeField]
    private Invader InvaderScript_;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            collider.gameObject.GetComponent<Player>().hit();
            recall();
		}
		else if (collider.gameObject.tag == "Wall")
		{
            recall();
		}
    }

    void recall()
    {
        if (InvaderScript_)
            InvaderScript_.recallProjectile();
        else
            Destroy(gameObject);
    }
}
