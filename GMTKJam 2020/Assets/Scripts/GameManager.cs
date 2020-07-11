using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameStates
{
    INIT = 0000,
    DRAFT = 1000,
    PLAY = 2000,
    ASSESS = 3000
}

public enum Resources
{
    NONE = 0000,
    HULL = 1000,
    FUEL = 2000,
    MORALE = 3000,
    PEOPLE = 4000
}
public struct Resource
{
    public ResourceData data;
    public int currentValue;
    public Resource(ResourceData data, int currentValue)
    {
        this.data = data;
        this.currentValue = currentValue;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<CardObject> playerHand = new List<CardObject> { };
    public List<CardObject> enemyHand = new List<CardObject> { };

    public List<CardBase> playerCardDatas = new List<CardBase> { };
    public List<CardBase> enemyCardDatas = new List<CardBase> { };
    public int playerHandMax = 8;
    public int enemyHandMax = 8;
    public ResourceData[] resources;
    public Dictionary<Resources, Resource> resourceDict = new Dictionary<Resources, Resource> { };
    // Start is called before the first frame update

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        foreach (ResourceData res in resources)
        {
            resourceDict.Add(res.type, new Resource(res, res.startValue));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddResource(Resources type, int amount)
    { // Use negative amounts for negative amounts
        Resource outRes;
        resourceDict.TryGetValue(type, out outRes);
        Mathf.Clamp(outRes.currentValue += amount, -1, outRes.data.maxValue);
        if (outRes.currentValue <= 0)
        {
            Defeat(outRes.data.type);
        }
    }
    public int GetResource(Resources type)
    {
        Resource outRes;
        resourceDict.TryGetValue(type, out outRes);
        return outRes.currentValue;
    }

    public void Defeat(Resources reason)
    {
        Debug.Log("LOST BECAUSE OF " + reason);
    }

    public void TriggerFlipToPlayerSide(GameObject target)
    {
        CardObject card = target.GetComponent<CardObject>();
        card.FlipToPlayerSide(true);
        AddCardToPlayerHand(card);
    }
    public void TriggerFlipToEnemySide(GameObject target)
    {
        CardObject card = target.GetComponent<CardObject>();
        card.FlipToPlayerSide(false);
        AddCardToEnemyHand(card);
    }

    public void AddCardToPlayerHand(CardObject card)
    {
        if (playerHand.Count < playerHandMax && !playerHand.Contains(card))
        {
            playerHand.Add(card);
            UIManager.instance.AddCardToHand(card.dataPlayer.side_, card);
            card.gameObject.SetActive(false);
        }
    }
    public void AddCardToEnemyHand(CardObject card)
    {
        if (enemyHand.Count < enemyHandMax && !enemyHand.Contains(card))
        {
            enemyHand.Add(card);
            UIManager.instance.AddCardToHand(card.dataEnemy.side_, card);
            card.gameObject.SetActive(false);
        }
    }
}
