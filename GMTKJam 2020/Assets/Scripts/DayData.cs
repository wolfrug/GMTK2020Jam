﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Day Data", order = 1)]
public class DayData : ScriptableObjectBase
{
    public int cardsDrawn = 2;
    public float draftTime = 60f;
    public CardBase[] enemyCards;
    public CardBase[] playerCards;
    public ObjectSound soundEffect;

    [Multiline]
    public string fluffText;

    public Dictionary<CardBase, float> allEnemyCards
    {
        get
        {
            Dictionary<CardBase, float> returnDict = new Dictionary<CardBase, float> { };
            foreach (CardBase card in enemyCards)
            {
                if (!returnDict.ContainsKey(card))
                {
                    returnDict.Add(card, card.weight_);
                };
            }
            return returnDict;
        }
    }
    public Dictionary<CardBase, float> allPlayerCards
    {
        get
        {
            Dictionary<CardBase, float> returnDict = new Dictionary<CardBase, float> { };
            foreach (CardBase card in playerCards)
            {
                if (!returnDict.ContainsKey(card))
                {
                    returnDict.Add(card, card.weight_);
                };
            }
            return returnDict;
        }
    }
}
