using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ChallengeTier/Ironbot")]
public class IronbotChallengeTier : ChallengeTier
{
    [Space, Header("Required Variables")]
    public int damage;
    public float damageFrequency;
    public float chargeTime;
    public float beamSize;
    public float effectDuration;

    public override void InitializeAbilityStats(Ability _ability)
    {
        SustainedAI ability = _ability as SustainedAI;
        for (int i = 0; i < ability.beams.Count; i++)
        {
            SustainedBeam sustained = ability.beams[i];
            sustained.damage = damage;
            sustained.effectFrequency = damageFrequency;
            ability.chargeTime = chargeTime;
            sustained.hitRadius = beamSize;

            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, beamSize);
            curve.AddKey(1, beamSize);
            Debug.Log("Curve Size  " + curve[1]);
            sustained.beamRenderer.widthCurve = curve;
            ability.effectLength = effectDuration;
        }
    }
}
