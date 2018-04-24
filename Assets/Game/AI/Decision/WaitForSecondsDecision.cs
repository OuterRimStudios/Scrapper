using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/WaitForSeconds")]
public class WaitForSecondsDecision : Decision
{
    public float waitTimeBeforeTransition;
    bool waitComplete = false;

    public override bool Decide(StateController controller)
    {
        Debug.Log("Looking for a decision " + controller.waitForSeconds_Waiting);

        if(!controller.waitForSeconds_Waiting)
        {
            controller.waitForSeconds_Waiting = true;
            Debug.Log("Begin to Wait");
            waitComplete = false;
            controller.StartACoroutine(WaitTime());
        }

        if (waitComplete)
        {
            Debug.Log("Done Waiting");
            controller.waitForSeconds_Waiting = false;
            return waitComplete;
        }
        else return false;
    }

    IEnumerator WaitTime()
    {
        Debug.Log("Waiting....");
        yield return new WaitForSeconds(waitTimeBeforeTransition);
        waitComplete = true;
    }
}
