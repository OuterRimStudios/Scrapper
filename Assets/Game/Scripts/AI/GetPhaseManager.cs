using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPhaseManager : ObjectReference
{
    public PhaseManager phaseManager;
    
	void Awake ()
    {
        SetReference(phaseManager);
	}
}
