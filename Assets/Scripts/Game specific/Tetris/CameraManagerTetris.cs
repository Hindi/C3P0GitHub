using UnityEngine;
using System.Collections;

public class CameraManagerTetris : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [SerializeField]
    Camera camera1;

    [SerializeField]
    Camera camera2;

    [SerializeField]
    Camera camera3;


    // We reference the main camera so we can disable this one especially
    [SerializeField]
    Camera main;

    public void setActiveCamera(Camera c)
    {
        main.gameObject.SetActive(false);
        camera1.gameObject.SetActive(false);
        camera2.gameObject.SetActive(false);
        camera3.gameObject.SetActive(false);
        c.gameObject.SetActive(true);
        Camera.SetupCurrent(c);
    }

    public void setParamId(int id)
    {
        switch (id)
        {
            case 0:
                setActiveCamera(camera1);
                break;
            case 1:
                setActiveCamera(camera2);
                break;
            case 2:
                setActiveCamera(camera3);
                break;
            default:
                break;
        }
       
    }
}
