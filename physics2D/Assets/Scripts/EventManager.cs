using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour {

    public delegate void TargetAction();
    public static event TargetAction targetReached;

    public void hitTarget() {
        if (targetReached != null)
            targetReached();
    }
}
