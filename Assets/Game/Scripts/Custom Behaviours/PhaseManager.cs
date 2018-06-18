using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
