using UnityEngine;
using System.Collections;

public class CoinBehaviour : MonoBehaviour {

	public AudioClip sound;

	public float translationSpeed = 12;
	public float translationDistance = 3;
	public float idleRotationSpeed = 250;
	public float onBlockRotationSpeed = 400;

    private Vector3 startPosition;
    private bool blockInstantiated = false;
    private bool goUp = true;

	// Use this for initialization
	void Start () 
    {
        startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
	}
	
	// Update is called once per frame
	void FixedUpdate () 
    {
        if (blockInstantiated) 
		{
			GetComponent<Animator>().SetBool("CoinTrig", true);
			//transform.Rotate (Vector3.back * Time.deltaTime * onBlockRotationSpeed);
            if (goUp)
            {
                transform.Translate(Vector3.up * Time.deltaTime * translationSpeed);
                if (Vector3.Distance(startPosition, transform.position) > translationDistance)
                {
                    goUp = !goUp;
                }
            }
            if (!goUp)
            {
                transform.Translate(Vector3.up * Time.deltaTime * (-translationSpeed));
                if (Vector3.Distance(startPosition, transform.position) < 1.5)
                {
                    Destroy(gameObject);
                }
            }
		}
		//else 
		//{
			//transform.Rotate (Vector3.back * Time.deltaTime * idleRotationSpeed);
		//}
	}

    public void jump()
	{
		AudioSource.PlayClipAtPoint (sound, transform.position);
        blockInstantiated = true;
        collider.enabled = false;
    }

	private void OnTriggerEnter(Collider collider)
	{
		AudioSource.PlayClipAtPoint (sound, transform.position);
		Destroy (gameObject);
	}
}
