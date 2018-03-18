using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAbility : Ability
{
    public int damage;


    Transform target;

    public override void ActivateAbility()
    {
        base.ActivateAbility();
        target = refManager.targetManager.GetClosestTarget(refManager.transform.position, refManager.enemyTag.ToString());

        if (target == null) return;
        if(Utility.CheckDistance(refManager.transform.position, target.position) < refManager.stats.attackRange)
        {
            Attack attack = new Attack();

            attack.Initialize(damage, refManager.enemyTag.ToString(), refManager.friendlyTag.ToString(), afterEffects);
            attack.SetTarget(target);
            for (int j = 0; j < GetActiveModules().Count; j++)
                attack.SetModule(GetActiveModules()[j]);

            attack.AttackTarget();
        }

        RemoveModules();
        TriggerCooldown();
    }

}

public class Attack : DamageTypes
{
    Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void AttackTarget()
    {
        //This is commeted out because only ai are using this at the moment and players dont have modules
        //ApplyModules(target.gameObject);
        target.GetComponent<Health>().TookDamage(damage);
    }
}
