using UnityEngine;
using System.Collections;

public class CameraManagerTetris : MonoBehaviour {
    


	// Use this for initialization
	void Start () {

        if (Application.isMobilePlatform)
        {
            main.transform.position = new Vector3(0, 10f, -10);
        }
        else
            main.transform.position = new Vector3(4.5f, 10f, -10);
        blur = main.GetComponent<Blur>() as Blur;
        noise = main.GetComponent<NoiseEffect>() as NoiseEffect;
            
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private Blur blur;
    private NoiseEffect noise;


    // We reference the main camera so we can access to its components more easely
    [SerializeField]
    Camera main;


    public void setParamId(int id)
    {
        switch (id)
        {
            case 0:
                //setActiveCamera(camera1);
                blur.enabled = true;
                noise.enabled = false;
                break;
            case 1:
                //setActiveCamera(camera2);
                noise.enabled = true;
                blur.enabled = false;
                break;
            case 2:
                //setActiveCamera(camera3);
                blur.enabled = true;
                noise.enabled = true;
                break;
            default:
                break;
        }
       
    }
}
