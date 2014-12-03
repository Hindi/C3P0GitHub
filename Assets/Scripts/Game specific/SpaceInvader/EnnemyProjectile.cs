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
            InvaderScript_.recallProjectile();
            collider.gameObject.GetComponent<Player>().hit();
        }
    }
}
