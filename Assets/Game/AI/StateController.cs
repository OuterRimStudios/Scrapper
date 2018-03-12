using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public EnemyReferenceManager enemyRefManager;
    public State currentState;                                              //The state that is currently active.
    public float updateStateFrequency;                                      //How often the AI will update it's state
    public State remainState;

    WaitForSeconds waitTime;                                                //Cached WaitForSeconds for optimization purposes
    bool updatingState;                                                     //Checks if it's time to update the state
    bool aiActive;                                                          //Allows us to not update the AI if it's not necessary for optimization purposes

	void Start ()
    {
        aiActive = true;
        waitTime = new WaitForSeconds(updateStateFrequency);
	}

	void Update ()
    {
        if (!aiActive) return;

		if(!updatingState)
        {
            updatingState = true;
            StartCoroutine(UpdateState());
        }
	}

    IEnumerator UpdateState()
    {
        currentState.UpdateState(this);                                      //Tell our State to update, pass it a refence to this instance
        yield return waitTime;
        updatingState = false;
    }

    public void TransitionToState(State nextState)
    {
        if(nextState != remainState)
        {
            currentState = nextState;
        }
    }
}
