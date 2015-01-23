using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

    [SerializeField]
    private GameObject projectile_;

    [SerializeField]
    int speed;

    [SerializeField]
    int projectileMaxDistance_;

    [SerializeField]
    float changeSizeCooldown;
    float lastTimeChangeSize;
    float goalScale;

    private int currentSpeed;

    [SerializeField]
    GUIStyle guiStyle;

	[SerializeField]
	float fireCooldown;
	float lastShotTime;

    private int paramId;

    private Rect labelRectScore;

    private int score;
    public int Score
    {
        get { return score; }
    }

    // Use this for initialization
    void Start()
    {
        double[] value = {1,3,5,7};
        Laws.setUniformValues(value);
        labelRectScore = new Rect(Screen.width - (Screen.width / 4), Screen.height / 25, Screen.width / 4, Screen.height / 20);
        paramId = 0;
        lastTimeChangeSize = Time.time;
        goalScale = transform.localScale.x;
        projectile_.SetActive(false);
        lastShotTime = Time.time;
	}

    public void onGameRestart()
    {
        score = 0;
        recallProjectile();
    }

    public void setDirection(bool right)
    {
        if (right)
            currentSpeed = speed;
        else
            currentSpeed = -speed;
    }

    public void setParamId(int id)
    {
        paramId = id;
    }

    public void stop()
    {
        currentSpeed = 0;
    }

	private bool canFire()
	{
		return (Time.time - lastShotTime > fireCooldown && !projectile_.activeSelf);
	}

    public void fire()
    {
		if (canFire ())
        {
			lastShotTime = Time.time;
            projectile_.SetActive(true);
            projectile_.transform.position = transform.position;
        }
    }

	public void hit()
	{
		EventManager<bool>.Raise (EnumEvent.GAMEOVER, false);
	}

    void move()
    {
        transform.position = new Vector3(transform.position.x + currentSpeed * Time.deltaTime, transform.position.y, transform.position.z);
    }

    public void enemyDestroyed()
    {
        score++;
    }

    public void recallProjectile()
    {
        projectile_.transform.position = transform.position;
        projectile_.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        guiStyle.fontSize = Mathf.RoundToInt(Responsive.baseFontSize * Screen.width / Responsive.baseWidth);
		move();
		if (projectile_.activeSelf)
		{
			if (Vector3.Distance(transform.position, projectile_.transform.position) > projectileMaxDistance_)
				recallProjectile();
		}

        {
            if (Time.time - lastTimeChangeSize > changeSizeCooldown)
            {
                switch (paramId)
                {
                    case 0:
                        goalScale = (float)Laws.uniforme();
                        break;
                    case 1:
                        goalScale = (float)Laws.gauss(5,3);
                        break;
                    case 2:
                        goalScale = (float)Laws.gauss(5, 1);
                        break;
                }
                goalScale = Mathf.Abs(goalScale);
                lastTimeChangeSize = Time.time;
                //transform.localScale = new Vector3(goalScale, 1, 1);
            }
		}
		transform.localScale -= new Vector3((transform.localScale.x - goalScale) / (transform.localScale.x + goalScale), 0, 0);
    }

    float calcBouncingForce(float delta)
    {
        return - Mathf.Sign(delta) * Mathf.Pow(delta / 3, 2);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            float delta = transform.position.x - collision.transform.position.x;
            collision.rigidbody.AddForce(new Vector3(calcBouncingForce(delta), 0, 0));
        }
    }

    void OnGUI()
    {
        GUI.Label(labelRectScore, "Score : " + score.ToString(), guiStyle);
    }
}