using UnityEngine;
using System.Collections;

public class GoombaBehaviour : MonoBehaviour 
{
    public AudioClip deathSound;
    private bool dieing = false;
    public bool Dieing
    {
        get { return dieing; }
    }

    private bool death = false;
    public bool Death
    {
        get { return death; }
    }
    Animator anim;

    [SerializeField]
    private Vector3 speed;
    void Awake()
    {
        
    }

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
        move();
    }

    public void move()
    {
        if(!dieing)
            transform.Translate(speed);
    }

	void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Player" && !Dieing)
        {
            collider.GetComponent<FirstPersonController>().die();
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("terrain"))
        {
            revertSpeed();
        }
	}

    public void revertSpeed()
    {
        speed = -speed;
    }

    public void die()
	{
        dieing = true;
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
