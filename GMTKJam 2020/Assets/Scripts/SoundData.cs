using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    NONE = 0000,
    AMBIENCE = 1000,
    PLAY_CARD = 2000,
    CARD_PICKUP = 2100,
    CARD_FLIP = 2200,
    CARD_DAMAGE = 3000,
    CARD_HEAL = 4000,
    BUTTON_CLICK = 4100,
}

[System.Serializable]
public class ObjectSound
{
    public SoundType type = SoundType.NONE;
    public List<AudioClip> sounds = new List<AudioClip> { };
}

[CreateAssetMenu(fileName = "Data", menuName = "Grid Object Sound Data", order = 1)]
public class SoundData : ScriptableObject
{
    public ObjectSound[] sounds;
}