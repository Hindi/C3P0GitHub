using UnityEngine;
using System.Collections;

/// <summary>The ball used in space invaders during breakout mode.</summary>
public class Ball : MonoBehaviour {

    /// <summary>Initial position.</summary>
    private Vector3 initPos;

    /// <summary>The player's script.</summary>
    [SerializeField]
    private Player playerScript_;

    /// <summary>Gives the initial impulsion in breakout mode.</summary>
    /// <returns>void</returns>
    public void switchToBreakOut()
    {
        rigidbody.AddForce(new Vector3(0, 0, 1.2f));
    }

    /// <summary>Reset the forces applied on the object.</summary>
    /// <returns>void</returns>
    private void resetForce()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }

    /// <summary>Switch back to normal mode.</summary>
    /// <returns>void</returns>
    public void switchToNormal()
    {
        resetForce();
    }

    /// <summary>If collides with and ennemy, destroys it.</summary>
    /// <returns>void</returns>
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            playerScript_.enemyDestroyed();
            collision.gameObject.GetComponent<Invader>().startDestruction();
        }
    }
}
