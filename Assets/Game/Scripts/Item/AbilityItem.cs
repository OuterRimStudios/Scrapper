using UnityEngine;

[CreateAssetMenu(menuName = "Item/AbilityItem")]
public class AbilityItem : Item
{
    public Ability ability;

    public void Start()
    {
        ItemName = ability.abilityName;
        ItemImage = ability.abilityIcon;
        ItemDescription = ability.abilityDescription;
    }
}
