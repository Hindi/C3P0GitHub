using UnityEngine;
using System.Collections;

public class GoombaBehaviour : MonoBehaviour 
{
    public AudioClip deathSound;

    private bool death = false;
    Animator anim;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update() 
    {
		if (isReadyToDie()) 
        {
			Destroy(gameObject);
		}
	}

    void FixedUpdate()
    {
        //anim.SetBool("walking", true);
    }

	void OnTriggerEnter(Collider collider)
	{
        if (collider.transform.tag == "Player" && !death)
        {
            die();
            collider.GetComponent<MarioControler>().bounce();
        }
	}

    private void die()
	{
        //Animation sprite
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        anim.SetBool("die", true);
	}

	private bool isReadyToDie()
	{
		//Check la fin de l'animation
		return death;
	}

    public void setReadyToDie()
    {
        death = true;
    }
}
