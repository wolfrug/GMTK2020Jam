﻿using UnityEngine;
using System.Collections;

public enum CardSide
{
    NONE = 0000,
    OCEAN = 1000,
    PLAYER = 2000,
}

[CreateAssetMenu(fileName = "Data", menuName = "Card Base", order = 1)]
public class CardBase : ScriptableObjectBase
{
    public Sprite image_;
    public CardSide side_;

    [Multiline]
    public string description_;
    [Multiline]
    public string longDescription_;
    public float weight_ = 1f;
    public CardEffect[] effects;
    public ObjectSound sounds;

}
