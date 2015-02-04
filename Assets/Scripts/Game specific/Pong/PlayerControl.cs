using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private float upScreen, downScreen;
    private PongManagerScript managerScript;
    private Vector3 originalScale;

    private Vector3 defaultPos;

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

    public void goDown()
    {
        if (transform.position.y >= -downScreen * managerScript.resizeHeight + 0.3)
        {
            transform.Translate(new Vector3(0, -1 * speed * Time.deltaTime * managerScript.resizeHeight, 0));
        }
    }

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
