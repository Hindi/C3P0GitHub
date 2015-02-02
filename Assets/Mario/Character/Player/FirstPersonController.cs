using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{

    public AudioClip jumpSound;
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 5.0f;
    public float jumpSpeed = 20.0f;
    public float upDownRange = 60.0f;
    public GameObject cameraHolder;
    
    private Animator anim;
    private float verticalVelocity = 0;
    private bool sliding = false;
    private bool jumping = false;
    private bool moving = false;
    private bool facingRight = true;

    CharacterController characterController;

    // Use this for initialization
    void Start()
    {
        //Screen.lockCursor = true;
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotation

        /*float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, rotLeftRight, 0);


        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
        */

        // Movement

        float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
        float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;

        if (!characterController.isGrounded)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime * 6;
        }
        else
        {
            jumping = false;
        }

        if (characterController.isGrounded && Input.GetButton("Jump"))
        {
            verticalVelocity = jumpSpeed;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            jumping = true;
        }

        sliding = false;

        if (Input.GetKey("right"))
        {
            moveRight();
            moving = true;
        }
        if(Input.GetKey("left"))
        {
            moveLeft();
            moving = true;
        }

        Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

        speed = transform.rotation * speed;
        if (speed.x == 0)
            moving = false;

        
		anim.SetBool ("moving", moving);
		anim.SetBool ("sliding", sliding);
		anim.SetBool ("jumping", jumping);

        characterController.Move(speed * Time.deltaTime);
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
	}

    void flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x = theScale.x * (-1);

        transform.localScale = theScale;
    }

    public void resetVerticalVelocity()
    {
        verticalVelocity = 0;
    }

    public void bounce()
    {
        verticalVelocity = jumpSpeed/2;
    }

    public void eatChamp()
    {
        cameraHolder.GetComponent<CameraController>().eatChamp();
    }
}
