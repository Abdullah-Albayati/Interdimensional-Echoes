using UnityEngine;

[CreateAssetMenu(fileName = "CreatureData", menuName = "Creatures/New Creature", order = 0)]
public class CreatureData : ScriptableObject
{
    public string creatureName = "New Creature";

    [Header("Description")]
    public string description;

    [Header("Attributes")]
    public float speed;

    [Header("Abilities")]
    public bool canPassThroughDoors;
    public bool isFasterInDarkness;
    public float speedIncreaseInDarkness;
    public bool isAttractedToLowSanity;


    [Header("Audio")]
    public AudioClip creatureIdleSound;
    public AudioClip creatureAttackSound;
    public AudioClip creatureMovingSound;
}
