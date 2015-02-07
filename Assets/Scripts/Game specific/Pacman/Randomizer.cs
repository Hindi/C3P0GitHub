using UnityEngine;
using System.Collections;

public class Randomizer : MonoBehaviour {

	[SerializeField]
	private GameObject circle;

	Color couleur;
	// Use this for initialization

	public void setColor(Color col){
		couleur = col;
	}

	void create(){
		GameObject dot = Instantiate (circle, transform.position, transform.rotation)as GameObject;
		dot.renderer.material.color = couleur;
		dot.transform.parent = transform;
		dot.transform.Translate(Random.Range(-10f, 10f), Random.Range(-5f, 5f), 1f, transform);
		//dot.transform.Translate((float)Laws.gauss(0,3.5), (float)Laws.gauss (0,3.5), 0.4f, transform);
	}

	
	void Start(){
		couleur = Color.gray;
	}
	void Update () {
			create();
	}

	void OnDisable(){
		EventManager.Raise(EnumEvent.MINIGAME_TERMINATE);
	}
}
