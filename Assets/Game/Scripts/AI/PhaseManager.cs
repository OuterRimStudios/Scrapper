using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class PhaseManager : MonoBehaviour
{
    public int phase;

    private void Start()
    {
        phase = 1;
    }

    public void SetPhase(int newPhase)
    {
        phase = newPhase;
    }

    public int GetCurrentPhase()
    {
        return phase;
    }
}

[System.Serializable]
public class SharedPhaseManager : SharedVariable<PhaseManager>
{
    public static implicit operator SharedPhaseManager(PhaseManager value) { return new SharedPhaseManager { Value = value }; }
}