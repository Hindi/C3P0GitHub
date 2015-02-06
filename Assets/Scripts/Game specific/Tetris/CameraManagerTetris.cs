using UnityEngine;
using System.Collections;

/// <summary>
/// This class is meant to handle camera effects for the game tetris.
/// Reminds that the effect is chosen at the beginning of a party by the player.
/// </summary>
public class CameraManagerTetris : MonoBehaviour {

    /// <summary>
    /// Shortcut for activation/desactivation of a camera effect.
    /// </summary>
    private Blur blur;

    /// <summary>
    /// Shortcut for activation/desactivation of a camera effect.
    /// </summary>
    private NoiseEffect noise;

    /// <summary>
    /// We reference the main camera so we can access to its components more easely
    /// </summary>
    [SerializeField]
    Camera main;

    /// <summary>
    /// Reference to another camera (used to stack some noise effect
    /// </summary>
    [SerializeField]
    Camera noiseCamera;

    // Use this for initialization
    /// <summary>
    /// Called when a CameraManagerTetris element is instantiate (when the scene is loading).
    /// We use it for initialisation. Ensures camera positions (different for mobile or pc)
    /// Shortcuts to access camera effect are created.
    /// </summary>
    /// <returns>void</returns>
    void Start()
    {

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
    void Update()
    {
        if (Input.GetKeyDown("space"))
            noiseCamera.gameObject.SetActive(true);
        if (Input.GetKeyUp("space"))
            noiseCamera.gameObject.SetActive(false);
    }


    /// <summary>
    /// Enable the right camera effect
    /// </summary>
    /// <param name="id">Id of the camera effect to enable</param>
    /// <returns>void</returns>
    public void setParamId(int id)
    {
        // suivant les cas on active la camera fille de la main (pour add du noise) avec :
        // noiseCamera.gameObject.SetActive(true);
        switch (id)
        {
            case -1:
                blur.enabled = false;
                noise.enabled = false;
                break;
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
