using UnityEngine;
using System.Collections;

public class Invader : MonoBehaviour {

    bool destroying;

    private float startTime;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    private GameObject animated;

    [SerializeField]
    private GameObject[] eyes;

	// Use this for initialization
	void Start () {
        destroying = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (destroying && Time.time - startTime > cooldown)
        {
            Destroy(animated);
            Destroy(gameObject);
        }
	}

    public void startDestruction()
    {
        destroying = true;
        animated.animation.Play();
        startTime = Time.time;
        collider.enabled = false;
        foreach (GameObject e in eyes)
            Destroy(e);
    }
}
