using UnityEngine;
using System.Collections;

/// <summary>The space invaders player script.</summary>
public class Player : MonoBehaviour
{
    /// <summary>Reference to the projectile object.</summary>
    [SerializeField]
    private GameObject projectile_;

    /// <summary>The player's moving speed.</summary>
    [SerializeField]
    int speed;

    /// <summary>Max distance the projectile can reach before being recalled.</summary>
    [SerializeField]
    int projectileMaxDistance_;

    /// <summary>The cooldown before the size is changed.</summary>
    [SerializeField]
    float changeSizeCooldown;

    /// <summary>The time when the size changed the last time.</summary>
    float lastTimeChangeSize;

    /// <summary>The scale to be reached.</summary>
    float goalScale;

    /// <summary>Old GUI style. TODO : replace with new UI system.</summary>
    [SerializeField]
    GUIStyle guiStyle;
    /// <summary>Rect of the score text.</summary>
    private Rect labelRectScore;

    /// <summary>Minimum time between two shots.</summary>
	[SerializeField]
	float fireCooldown;

    /// <summary>The last time the player shot.</summary>
	float lastShotTime;

    /// <summary>Param id that influences the player scale.</summary>
    private int paramId;

    /// <summary>Current score.</summary>
    private int score;
    public int Score
    {
        get { return score; }
    }

    /// <summary>Init positions and timers.</summary>
    /// <returns>void</returns>
    void Start()
    {
        labelRectScore = new Rect(Screen.width - (Screen.width / 4), Screen.height / 25, Screen.width / 4, Screen.height / 20);
        paramId = 0;
        lastTimeChangeSize = Time.time;
        goalScale = transform.localScale.x;
        projectile_.SetActive(false);
        lastShotTime = Time.time;
	}

    /// <summary>Recall projectile and reset score on restart.</summary>
    /// <returns>void</returns>
    public void onGameRestart()
    {
        score = 0;
        recallProjectile();
    }

    /// <summary>Set the param id.</summary>
    /// <returns>void</returns>
    public void setParamId(int id)
    {
        paramId = id;
    }

    /// <summary>Checks if the player can shoot.</summary>
    /// <returns>True if the player can fire.</returns>
	private bool canFire()
	{
		return (Time.time - lastShotTime > fireCooldown && !projectile_.activeSelf);
	}

    /// <summary>Check if the player can shot, then does it.</summary>
    /// <returns>void</returns>
    public void fire()
    {
		if (canFire ())
        {
			lastShotTime = Time.time;
            projectile_.SetActive(true);
            projectile_.transform.position = transform.position;
        }
    }

    /// <summary>Called when it by an enemy.</summary>
    /// <returns>void</returns>
	public void hit()
	{
		EventManager<bool>.Raise (EnumEvent.GAMEOVER, false);
	}

    /// <summary>Update the player's position.</summary>
    /// <returns>void</returns>
    public void move(int dir)
    {
        transform.position = new Vector3(transform.position.x + speed * dir * Time.deltaTime, transform.position.y, transform.position.z);
    }

    /// <summary>Update the score when enemy is destroyed.</summary>
    /// <returns>void</returns>
    public void enemyDestroyed()
    {
        updateScore();
    }

    /// <summary>Increase the score and notify the game sate of the change.</summary>
    /// <returns>void</returns>
    private void updateScore()
    {
        score++;
        EventManager<int>.Raise(EnumEvent.UPDATEGAMESCORE, score);
    }

    /// <summary>Recall projectile and desactivates it.</summary>
    /// <returns>void</returns>
    public void recallProjectile()
    {
        projectile_.transform.position = transform.position;
        projectile_.SetActive(false);
    }

    /// <summary>Update the projectile's position and the player's scale..</summary>
    /// <returns>void</returns>
    void Update()
    {
        guiStyle.fontSize = Mathf.RoundToInt(Responsive.baseFontSize * Screen.width / Responsive.baseWidth);
		if (projectile_.activeSelf)
		{
			if (Vector3.Distance(transform.position, projectile_.transform.position) > projectileMaxDistance_)
				recallProjectile();
		}

        if (Time.time - lastTimeChangeSize > changeSizeCooldown)
        {
            switch (paramId)
            {
                case 0:
                    goalScale = (float)Laws.uniforme(1, 15);
                    break;
                case 1:
                    goalScale = (float)Laws.exponential(0.5f);
                    break;
                case 2:
                    goalScale = (float)Laws.gauss(8, 2);
                    break;
            }
            cutScale();
            lastTimeChangeSize = Time.time;
            //transform.localScale = new Vector3(goalScale, 1, 1);
        }
		transform.localScale -= new Vector3((transform.localScale.x - goalScale) / (transform.localScale.x + goalScale), 0, 0);
    }

    /// <summary>Cut too high or too low scale value.</summary>
    /// <returns>void</returns>
    private void cutScale()
    {
        if (goalScale > 15)
            goalScale = 15;
        else if (goalScale <= 1)
            goalScale = 1;
    }

    /// <summary>Bounce force applied to the ball on bounce.</summary>
    /// <returns>void</returns>
    float calcBouncingForce(float delta)
    {
        Debug.Log(-Mathf.Sign(delta) * Mathf.Pow(delta / 3, 2));
        return - Mathf.Sign(delta) * Mathf.Pow(delta / 3, 2);
    }

    /// <summary>The ball bounce back on collision.</summary>
    /// <returns>void</returns>
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