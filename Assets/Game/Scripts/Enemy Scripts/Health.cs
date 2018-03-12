using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int baseHealth;

    protected int health;
    protected ReferenceManager refManager;
    Coroutine dot;

    protected virtual void Start()
    {
        health = baseHealth;
        refManager = GetComponent<ReferenceManager>();
    }

    public void Heal(int healAmount)
    {
        if (health + healAmount <= baseHealth)
            health += healAmount;
        else
            health = baseHealth;
    }

    public virtual void TookDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Died();
        }
    }

    protected virtual void Died()
    {
        Destroy(gameObject);
    }

    #region DOT
    public void ApplyDOT(int dotDamage, float dotLength)
    {
        if (dot != null)
            StopCoroutine(dot);

        dot = StartCoroutine(DOT(dotDamage, dotLength));
    }

    IEnumerator DOT(int _dotDamage, float _dotLength)
    {
        for (int i = 0; i < _dotLength; i++)
        {
            TookDamage(_dotDamage);
            print("Dealing " + _dotDamage + " damage to enemy");
            yield return new WaitForSeconds(1);
        }
    }
    #endregion
}