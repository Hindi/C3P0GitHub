using UnityEngine;
using System.Collections;

public class MoveToPocman : MonoBehaviour {
	GameObject pocman;

	PacMove pocMove;


	Vector3 startingPosition;
	Vector3 startingForward;
	Vector3 targetPosition;

	bool isTarget = false;
	bool gotHit = false;
	bool keyPressed = false;

	// Use this for initialization
	void Start () {
		pocman = GameObject.FindGameObjectWithTag("Pacman");
		pocMove = pocman.GetComponent<PacMove>();

		startingPosition = transform.position;
		startingForward = transform.position + 25 * transform.forward;
		targetPosition = startingPosition;
	}




	public void MoveTo(GameObject ghost){
		if (!gotHit){
			targetPosition = ghost.transform.position;
			isTarget = true;
			gotHit = true;
		}
	}


	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.Return)){
			isTarget = false;
			gotHit = false;
			keyPressed = true;

		}
		if (isTarget){
			Time.timeScale = 0;
			transform.LookAt(Vector3.Lerp(transform.position + transform.forward, targetPosition, Time.unscaledDeltaTime));
			if(Vector3.Distance(transform.position, pocman.transform.position) > 0.5f){
				transform.position = Vector3.Lerp(transform.position, pocman.transform.position, 2 * Time.unscaledDeltaTime);
			}
		}
		else {

			targetPosition = startingPosition;
			transform.LookAt(Vector3.Lerp(transform.position + transform.forward, startingForward, Time.unscaledDeltaTime));
			transform.position = Vector3.Lerp(transform.position, startingPosition, 2 * Time.unscaledDeltaTime);
			if (keyPressed && Vector3.Distance(transform.position, startingPosition) < 0.5f){
				Time.timeScale = 1;

			}
		}	



	}
}