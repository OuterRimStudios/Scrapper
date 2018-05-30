using UnityEngine;

public class Item : ScriptableObject
{
    public string ItemName { get; protected set; }
    public Sprite ItemImage { get; protected set; }
    public string ItemDescription { get; protected set; }
    public int DropChance { get; protected set; }
}
