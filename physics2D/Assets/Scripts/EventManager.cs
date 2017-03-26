using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

    // When player hit the target
    public delegate void TargetAction();
    public static event TargetAction hitTargetEvent;

    public void hitTarget() {
        if (hitTargetEvent != null)
            hitTargetEvent();
    }

    // When player crashes the space ship
    public delegate void CrashAction();
    public static event CrashAction crashEvent;

    public void crash() {
        if (crashEvent != null)
            crashEvent();
    }

    // When player runs out of the game limits
    public delegate void OutOfLimitsAction();
    public static event OutOfLimitsAction outOfLimitsEvent;

    public void outOfLimits() {
        if (outOfLimitsEvent != null)
            outOfLimitsEvent();
    }

    // When player runs out of time to reach the target
    public delegate void OutOfTimeAction();
    public static event OutOfTimeAction outOfTimeEvent;

    public void outOfTime() {
        if (outOfTimeEvent != null)
            outOfTimeEvent();
    }
}
