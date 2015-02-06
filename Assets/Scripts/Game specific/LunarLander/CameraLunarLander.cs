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

    private bool isFar;

	// Use this for initialization
	void Start () {
        isFar = true;
        initialPosition = Camera.main.transform.position;
	}

    public void resetCameraPosition(Vector3 pos)
    {
        isFar = true;
        transform.position = pos;
    }
	
    public void zoom()
    {
        camera.orthographicSize = 6;
        isFar = false;
    }

	// Update is called once per frame
	void Update () {
        if (isFar)
        {
            if(camera.WorldToScreenPoint(player.transform.position).y < Screen.height / 3)
            {
                zoom();
            }
        }
        else
        {
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
}
