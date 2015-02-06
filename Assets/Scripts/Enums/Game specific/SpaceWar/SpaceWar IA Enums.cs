using UnityEngine;
using System.Collections;

/// <summary>
/// Enum used to store the general activity state of the AI
/// </summary>
public enum SWIAState {
    RANDOM,
    MOVE_RANDOMLY,
    ATTACK_RANDOMLY,
    MOVE_TOWARDS_PLAYER,
    ATTACK_PLAYER,
    DODGE_ATTACKS
}

/// <summary>
/// Enum used to store the current movement performed by the AI
/// </summary>
public enum SWIAAction
{
    MOVE_FORWARD,
    MOVE_TURN_LEFT,
    MOVE_TURN_RIGHT,
    TURN_LEFT,
    TURN_RIGHT,
    NOTHING
}
