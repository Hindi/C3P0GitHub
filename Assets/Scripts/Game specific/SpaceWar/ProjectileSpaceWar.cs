using UnityEngine;
using System.Collections;

public class ProjectileSpaceWar : MonoBehaviour {

    public void exitZone()
    {
        rigidbody2D.isKinematic = true;
        rigidbody2D.isKinematic = false;
        gameObject.SetActive(false);
    }
}
