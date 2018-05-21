using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/State")]
public class State : ScriptableObject
{
    public AIAction[] actions;
    public Transition[] transitions;

    public void UpdateState(StateController controller)
    {
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateController controller)
    {
        if (actions.Length <= 0) return;
        for(int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }

    public void CheckTransitions(StateController controller)
    {
        for(int i = 0; i < transitions.Length; i++)
        {
            bool decisionSucceeded = transitions[i].decision.Decide(controller);        //Check transitions

            if (decisionSucceeded)                                          
                controller.TransitionToState(transitions[i].trueState);                 //If decisions is true, transition to true state
            else
                controller.TransitionToState(transitions[i].falseState);                //Else transition to false state
        }
    }
}
