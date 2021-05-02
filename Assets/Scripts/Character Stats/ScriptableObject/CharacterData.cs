using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Data",menuName = "Character Stats/Data")]
public class CharacterData : ScriptableObject
{
    [Header("Stats Info")] 
    public int maxHealth;

    public int currentHealth;

    public int baseDefense;

    public int currentDefense;
}
