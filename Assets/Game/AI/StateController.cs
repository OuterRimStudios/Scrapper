using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public AIReferenceManager enemyRefManager;
    public State currentState;                                              //The state that is currently active.
    public float updateStateFrequency;                                      //How often the AI will update it's state
    public State remainState;
    [HideInInspector]public bool isIdle = true;
    [HideInInspector] public float stateTimeElapsed;

    WaitForSeconds waitTime;                                                //Cached WaitForSeconds for optimization purposes
    bool updatingState;                                                     //Checks if it's time to update the state
    [HideInInspector] public bool aiActive;                                                          //Allows us to not update the AI if it's not necessary for optimization purposes

	void Start ()
    {
        isIdle = true;
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
            print(currentState.name + " -> " + nextState.name);
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
    }

    public void ToggleIdle()
    {
        isIdle = !isIdle;
    }
}
