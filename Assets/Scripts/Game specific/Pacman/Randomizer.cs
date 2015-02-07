using UnityEngine;
using System.Collections;

public class Randomizer : MonoBehaviour {

	[SerializeField]
	private GameObject circle;

	int paramId;
	
	Color couleur;
	
	public void setColor(Color col){
		couleur = col;
	}

	public void setParamId(int id){
		paramId = id;
	}

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
	
	void Start(){
		couleur = Color.gray;
	}
	void Update () {
		if (Time.timeScale > 0 ){
			create();
		}
	}

	void OnDisable(){
		EventManager.Raise(EnumEvent.MINIGAME_TERMINATE);
	}
	
}
