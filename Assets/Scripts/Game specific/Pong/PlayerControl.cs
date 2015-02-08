using UnityEngine;
using System.Collections;

/// <summary>
/// Script on the player paddle
/// </summary>
public class PlayerControl : MonoBehaviour {

    /// <summary>
    /// The speed at which the paddle goes
    /// </summary>
    [SerializeField]
    private float speed;
    /// <summary>
    /// The size of the screen
    /// </summary>
    [SerializeField]
    private float upScreen;
    [SerializeField]
    private float downScreen;
    /// <summary>
    /// The pong script to get resize values
    /// </summary>
    private PongManagerScript managerScript;
    /// <summary>
    /// The original scale of the paddle
    /// </summary>
    private Vector3 originalScale;
    /// <summary>
    /// The position at restart
    /// </summary>
    private Vector3 defaultPos;

    /// <summary>
    /// Sets the paddle ready for next round
    /// </summary>
    /// <param name="resizeWidth">1 because deprecated</param>
    /// <param name="resizeHeight">1 because deprecated</param>
    public void onRestart(float resizeWidth, float resizeHeight)
    {
        transform.position = new Vector3(defaultPos.x * resizeWidth, defaultPos.y * resizeHeight, 0);
        transform.localScale = new Vector3(originalScale.x * resizeWidth, originalScale.y * resizeHeight, originalScale.z);
    }

	// Use this for initialization
	void Start () {
        defaultPos = transform.position;
        managerScript = GameObject.FindGameObjectWithTag("PongManagerScript").GetComponent<PongManagerScript>();
        originalScale = transform.localScale;
	}

    /// <summary>
    /// Moves the paddle down
    /// </summary>
    public void goDown()
    {
        if (transform.position.y >= -downScreen * managerScript.resizeHeight + 0.3)
        {
            transform.Translate(new Vector3(0, -1 * speed * Time.deltaTime * managerScript.resizeHeight, 0));
        }
    }

    /// <summary>
    /// Moves the paddle up
    /// </summary>
    public void goUp()
    {
        if (transform.position.y <= upScreen * managerScript.resizeHeight - 0.3)
        {
            transform.Translate(new Vector3(0, speed * Time.deltaTime * managerScript.resizeHeight, 0));
        }
    }
	
	// Update is called once per frame
	void Update () {
	}
}
