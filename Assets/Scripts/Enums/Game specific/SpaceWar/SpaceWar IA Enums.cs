using UnityEngine;
using System.Collections;

public enum SWIAState {
    RANDOM,
    MOVE_RANDOMLY,
    ATTACK_RANDOMLY,
    MOVE_TOWARDS_PLAYER,
    ATTACK_PLAYER,
    DODGE_ATTACKS
}

public enum SWIAAction
{
    MOVE_FORWARD,
    MOVE_TURN_LEFT,
    MOVE_TURN_RIGHT,
    TURN_LEFT,
    TURN_RIGHT,
    NOTHING
}
