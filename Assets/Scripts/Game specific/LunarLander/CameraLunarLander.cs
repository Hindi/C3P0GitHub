using UnityEngine;
using System.Collections;

public class CameraLunarLander : MonoBehaviour {

    [SerializeField]
    private GameObject player;

    private Vector3 initialPosition;
    public Vector3 InitialPosition
    {
        get { return initialPosition; }
        set { initialPosition = value; }
    }

    private bool zooming;
    public bool Zooming
    {
        get { return zooming; }
        set { zooming = value; }
    }

	// Use this for initialization
	void Start () {
        zooming = false;
        initialPosition = Camera.main.transform.position;
	}

    public void resetCameraPosition(Vector3 pos)
    {
        zooming = false;
        transform.position = pos;
        camera.orthographicSize = 9;
    }
	
    public void zoom()
    {
        camera.orthographicSize -= 3 * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.transform.position.x, transform.position.y, transform.position.z), 0.05f);
    }

	// Update is called once per frame
	void Update () {
        if(zooming)
        {
            if (camera.orthographicSize > 6)
                zoom();
            else
                zooming = false;
        }

        if (camera.WorldToScreenPoint(player.transform.position).y < Screen.height / 2)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
        if (camera.WorldToScreenPoint(player.transform.position).y > 7 * Screen.height / 8)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
        }
	}
}
