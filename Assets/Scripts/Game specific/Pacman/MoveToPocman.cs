using UnityEngine;
using System.Collections;

public class MoveToPocman : MonoBehaviour {
	GameObject pocman;
	GameObject blanky;
	GameObject panky;
	GameObject anky;
	GameObject clode;

	PacMove pocMove;
	BlankyMove blankyMove;
	PankyMove pankyMove;
	AnkyMove ankyMove;
	ClodeMove clodeMove;


	Vector3 startingPosition;
	Vector3 startingForward;
	Vector3 targetPosition;

	bool isTarget = false;

	// Use this for initialization
	void Start () {
		pocman = GameObject.FindGameObjectWithTag("Pacman");
		pocMove = pocman.GetComponent<PacMove>();

		blanky = GameObject.Find("Blanky");
		blankyMove = blanky.GetComponent<BlankyMove>();

		panky = GameObject.Find("Panky");
		pankyMove = panky.GetComponent<PankyMove>();

		anky = GameObject.Find("Anky");
		ankyMove = anky.GetComponent<AnkyMove>();

		clode = GameObject.Find("Clode");
		clodeMove = clode.GetComponent<ClodeMove>();

		startingPosition = transform.position;
		startingForward = transform.position + 25 * transform.forward;
		targetPosition = startingPosition;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			isTarget = true;
			pocMove.moving(false);
			blankyMove.moving(false);
			pankyMove.moving(false);
			ankyMove.moving(false);
			clodeMove.moving(false);
		}
		if (Input.GetKeyDown(KeyCode.Return)){
			isTarget = false;
			pocMove.moving(true);
			blankyMove.moving(true);
			pankyMove.moving(true);
			ankyMove.moving(true);
			clodeMove.moving(true);
		}
		if (isTarget){
			targetPosition = pocman.transform.position;
			transform.LookAt(Vector3.Lerp(transform.position + transform.forward, pocman.transform.position + pocMove.getCurDir(), Time.deltaTime));
		}
		else {
			targetPosition = startingPosition;
			transform.LookAt(Vector3.Lerp(transform.position + transform.forward, startingForward, Time.deltaTime));
		}	
		transform.position = Vector3.Lerp(transform.position, targetPosition, 2 * Time.deltaTime);
	}
}