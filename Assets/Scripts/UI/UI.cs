using UnityEngine;
using System.Collections;

public class UI : MonoBehaviour {

	[SerializeField]
    private Canvas pauseMenu;
    [SerializeField]
    private Canvas paramMenu;

    private Canvas currentCanvas;

	// Use this for initialization
	void Start () {
		EventManager<bool>.AddListener (EnumEvent.PAUSEGAME, onGamePaused);
	}

	public void onGamePaused(bool b)
	{
        if (b)
            updateCurrentCanvas(pauseMenu);
        else
            closeMenus();
	}

    public void switchToParam()
    {
        updateCurrentCanvas(paramMenu);
    }

    private void updateCurrentCanvas(Canvas newCanvas)
    {
        if (currentCanvas)
            currentCanvas.gameObject.SetActive(false);
        currentCanvas = newCanvas;
        currentCanvas.gameObject.SetActive(true);
    }

    private void closeMenus()
    {
        pauseMenu.gameObject.SetActive(false);
        paramMenu.gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
