using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController : MonoBehaviour
{
    [SerializeField]
    private MarioScoreManager scoreManager;

    public AudioClip jumpSound;
    public float movementSpeed = 5.0f;
    public float mouseSensitivity = 5.0f;
    public float jumpSpeed = 15.0f;
    public float upDownRange = 60.0f;
    public GameObject cameraHolder;
    
    private Animator anim;
    private float verticalVelocity = 0;
    private bool sliding = false;
    private bool jumping = false;
    private bool moving = false;
    private bool facingRight = true;
    private bool finishing = false;

    private bool dieing = false;
    public bool Dieing
    {
        get { return dieing; }
    }

    [SerializeField]
    private Sprite death;

    [SerializeField]
    private Transform flagBasePosition;

    private Vector3 startPosition;

    CharacterController characterController;
    float sideSpeed;

    // Use this for initialization
    void Start()
    {
        //Screen.lockCursor = true;
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
    }

    public void restart()
    {
        transform.position = startPosition;
        dieing = false;
        anim.enabled = true;
        finishing = false;
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
        if(finishing)
        {
            transform.position = Vector3.MoveTowards(transform.position, flagBasePosition.position, 5 * Time.deltaTime);
            if(Vector3.Distance(transform.position, flagBasePosition.position) < 0.1f)
            {
                //finishing = false;
            }
        }
        else
        {
            float forwardSpeed = 0;//Input.GetAxis("Vertical") * movementSpeed;

            Vector3 speed = new Vector3(sideSpeed, verticalVelocity, forwardSpeed);

            speed = transform.rotation * speed;
            if (speed.x == 0)
                moving = false;

            if (!characterController.isGrounded)
            {
                verticalVelocity += Physics.gravity.y * Time.deltaTime * 6;
            }
            else
            {
                jumping = false;
            }
            sliding = false;

           /* if (characterController.isGrounded && Input.GetButton("Jump"))
            {
                verticalVelocity = jumpSpeed;
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                jumping = true;
            }


            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveRight();
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveLeft();
            }*/
            if (transform.position.y < -10)
            {
                EventManager<bool>.Raise(EnumEvent.GAMEOVER, false);
            }

            anim.SetBool("moving", moving);
            //anim.SetBool ("sliding", sliding);
            anim.SetBool("jumping", jumping);

            characterController.Move(speed * Time.deltaTime);
        }

        
    }

    public void jump()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = jumpSpeed;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            jumping = true;
        }
    }

    public void moveRight ()
	{
        if (!finishing)
        {
            if (!facingRight)
                flip();
            moving = true;
            move();
        }
        else
            stop();
	}

    public void moveLeft()
	{
        if(!finishing)
        {
            if (facingRight)
                flip();
            move();
        }
        else
            stop();
	}

    void move()
    {
        moving = true;
        sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
    }

    public void stop()
    {
        sideSpeed = 0;
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
        verticalVelocity = jumpSpeed/1.5f;
    }

    public void eatChamp()
    {
        cameraHolder.GetComponent<CameraController>().eatChamp();
    }

    public void die()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        dieing = true;
        anim.enabled = false;
        verticalVelocity = jumpSpeed / 1.5f;
        GetComponent<SpriteRenderer>().sprite = death;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.transform.tag == "Objective")
        {
            finishing = true;
        }
    }
}
