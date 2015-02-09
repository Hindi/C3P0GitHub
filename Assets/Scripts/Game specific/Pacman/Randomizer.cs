using UnityEngine;
using System.Collections;

public class Randomizer : MonoBehaviour {

	/// <summary>
	/// The circle gameObject
	/// </summary>
	[SerializeField]
	private GameObject circle;

	/// <summary>
	/// The parameter identifier.
	/// </summary>
	int paramId;

	/// <summary>
	/// The desired color
	/// </summary>
	Color couleur;


	/// <summary>
	/// Sets the color.
	/// </summary>
	/// <param name="col">The desired color.</param>
	public void setColor(Color col){
		couleur = col;
	}

	/// <summary>
	/// Sets the parameter identifier.
	/// </summary>
	/// <param name="id">The identifier of the parameter.</param>
	public void setParamId(int id){
		paramId = id;
	}

	/// <summary>
	/// Create dots.
	/// </summary>
	void create(){
		GameObject dot = Instantiate (circle, transform.position, transform.rotation)as GameObject;
		dot.renderer.material.color = couleur;
		dot.transform.parent = transform;
		switch(paramId){
		case 0 :
			dot.transform.Translate(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 1f, transform);
			break;
		case 1:
			dot.transform.Translate((float)Laws.gauss(0,3f), (float)Laws.gauss (0,3f), 1f, transform);
			break;
		case 2:
			Vector2 corr = Laws.gaussCorrelated(2);
			dot.transform.Translate(corr.x, corr.y, 1f, transform);
			break;
		}
	}

	/// <summary>
	/// SCalled when the scene loads
	/// </summary>
	void Start(){
		couleur = Color.gray;
	}

	/// <summary>
	/// Called after each frame
	/// </summary>
	void Update () {
		if (Time.timeScale > 0 ){
			create();
		}
	}

	/// <summary>
	/// Called when this script is disabled
	/// </summary>
	void OnDisable(){
		EventManager.Raise(EnumEvent.MINIGAME_TERMINATE);
	}
	
}
