using UnityEngine;
using System.Collections;

public class MarioControler : MonoBehaviour {

    public AudioClip jumpSound;
	public float maxSpeed = 0.00000002f;
    public float topSpeed = 0.0003f;

	bool facingRight = true;
	Animator anim;
	Vector3 vit;
	private bool sliding=false;
    private bool jumping = false;
	private bool moving = false;

	void Start () 
    {
		anim = GetComponent<Animator> ();
	}

	// Update is called once per frame

	void FixedUpdate ()
    {
        if (Input.GetKeyDown("space"))
        {
            jump();
        }
        if (Input.GetKeyDown("up"))
        {
            smallJump();
        }

        rigidbody.AddRelativeForce(Vector3.down * 40f, ForceMode.Acceleration);

		sliding = false;

		if (Input.GetKey("right"))
		{
			moveRight();
			moving = true;
		}
		else 
        {

            if (Input.GetKey("left"))
            {
                moveLeft();
                moving = true;
            }
            else
            {
                moving = false;
            }
		
		}
        if (rigidbody.velocity.x > topSpeed || rigidbody.velocity.x < -topSpeed)
        {
            Vector3 TSpeed = rigidbody.velocity.normalized * topSpeed;
            rigidbody.velocity = new Vector3(TSpeed.x, rigidbody.velocity.y, TSpeed.z); 
        }
        if (rigidbody.velocity.y < -0.5 || rigidbody.velocity.y > 0.5)
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }

		anim.SetBool ("moving", moving);
		anim.SetBool ("sliding", sliding);
		anim.SetBool ("jumping", jumping);
	}

	void moveRight ()
	{
		if (!facingRight) 
		{
			flip();
            if (moving)
            {
                sliding = true;
            }
		}
        rigidbody.AddRelativeForce(Vector3.right * maxSpeed * Time.deltaTime, ForceMode.Acceleration);

	}

	void moveLeft ()
	{
		if (facingRight) 
		{
			flip();
            if (moving)
            {
                sliding = true;
            }
        }
        rigidbody.AddRelativeForce(Vector3.left * maxSpeed * Time.deltaTime, ForceMode.Acceleration);
	}

	void flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x = theScale.x * (-1);

		transform.localScale = theScale;
    }

    public void jump()
    {
        rigidbody.AddRelativeForce(Vector3.up * 1000, ForceMode.Acceleration);
        jumping = true;
        AudioSource.PlayClipAtPoint(jumpSound, transform.position);
    }

    public void smallJump()
    {
        rigidbody.AddRelativeForce(Vector3.up * 630, ForceMode.Acceleration);
        jumping = true;
        AudioSource.PlayClipAtPoint(jumpSound, transform.position);
    }

    public void bounce()
    {
        rigidbody.AddRelativeForce(Vector3.up * 400, ForceMode.Acceleration);
    }

    public void pushDown()
    {
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
        rigidbody.AddRelativeForce(Vector3.down * 250, ForceMode.Acceleration);
    }

}
