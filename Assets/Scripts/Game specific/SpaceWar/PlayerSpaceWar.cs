using UnityEngine;
using System.Collections;

/// <summary>
/// Script used on the player's ship in SpaceWar
/// </summary>
public class PlayerSpaceWar : Spaceship {

	// Use this for initialization
    void Start()
    {

	}

	
	// Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (isEnd)
        {
            if (Time.time - initEndTimer >= 2)
            {
                isEnd = false;
                EventManager<bool, int>.Raise(EnumEvent.SPACESHIPDESTROYED, true, -1 * scoreMult);
            }
        }
	}

    /// <summary>
    /// Called when the ship is hit by a projectile or the center spiral
    /// </summary>
    public override void onHit()
    {
        base.onHit();
    }

    /// <summary>
    /// Resets the ship's properties to be ready for the next round
    /// </summary>
    public override void onRestart()
    {
        base.onRestart();
    }
}
