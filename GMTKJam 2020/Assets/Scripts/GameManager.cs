using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

[System.Serializable]
public class GameStateEvent : UnityEvent<GameState> { }

public enum GameStates
{
    NONE = 0000,
    INIT = 1000,
    DRAFT = 2000,
    PLAY = 3000,
    ASSESS = 4000,
    DEFEAT = 5000,
    WIN = 6000,

}

public enum Resources
{
    NONE = 0000,
    HULL = 1000,
    FUEL = 2000,
    MORALE = 3000,
    PEOPLE = 4000
}
[System.Serializable]
public class Resource
{
    public ResourceData data;
    public int currentValue;
    public Resource(ResourceData data, int currentValue)
    {
        this.data = data;
        this.currentValue = currentValue;
    }
}

[System.Serializable]
public class GameState
{
    public GameStates state;
    public GameStateEvent evtStart;
    public GameStates nextState;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameState[] gameStates;
    private GameState currentState;
    public int dayCount = 0;
    public ResourceData[] resources;
    public DayData[] days;
    private Dictionary<Resources, Resource> resourceDict = new Dictionary<Resources, Resource> { };
    private Dictionary<GameStates, GameState> gameStateDict = new Dictionary<GameStates, GameState> { };
    private Dictionary<int, DayData> daysDict = new Dictionary<int, DayData> { };
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
        foreach (GameState states in gameStates)
        {
            gameStateDict.Add(states.state, states);
        }
        currentState = gameStateDict[GameStates.INIT];
        gameStateDict[currentState.state].evtStart.Invoke(currentState);
    }
    public void Init()
    {
        foreach (ResourceData res in resources)
        {
            resourceDict.Add(res.type, new Resource(res, res.startValue));
            Debug.Log("Added resource " + res.type + " with value " + resourceDict[res.type].currentValue);
        }
        for (int i = 0; i < days.Length; i++)
        {
            daysDict.Add(i, days[i]);
        }
        UIManager.instance.Init();
        NextState();
    }

    public void NextState()
    {
        if (currentState.nextState != GameStates.NONE)
        {
            currentState = gameStateDict[currentState.nextState];
            gameStateDict[currentState.state].evtStart.Invoke(currentState);
            Debug.Log("Changed states to " + currentState.state);
        }
    }
    public GameStates GameState
    {
        get
        {
            if (currentState != null)
            {
                return currentState.state;
            }
            else
            {
                return GameStates.NONE;
            }
        }
        set
        {
            currentState = gameStateDict[value];
        }
    }


    public void EndDay()
    {
        NextState();
    }
    public void StartNextDay()
    {
        dayCount++;
        if (dayCount >= 8)
        {
            WinGame();
        }
        else
        {
            UIManager.instance.StartNewDay();
            CardGameManager.instance.StartNextDay();
        };
    }

    void WinGame()
    {
        GameState = GameStates.WIN;
        Debug.Log("Victory!!");
    }

    public DayData GetCurrentDay()
    {
        if (dayCount < days.Length)
        {
            return daysDict[dayCount];
        }
        else
        {
            return daysDict[days.Length - 1];
        }
    }

    public void AddResource(Resources type, int amount)
    { // Use negative amounts for negative amounts
        Resource outRes;
        resourceDict.TryGetValue(type, out outRes);
        Mathf.Clamp(outRes.currentValue += amount, -99, outRes.data.maxValue);
        UIManager.instance.UpdateResource(type);
        if (outRes.currentValue <= 0)
        {
            //Defeat(outRes.data.type);
        }
        Debug.Log("Changed resource " + type.ToString() + " to " + outRes.currentValue);
    }
    public int GetResource(Resources type)
    {
        Resource outRes;
        resourceDict.TryGetValue(type, out outRes);
        return outRes.currentValue;
    }

    public bool CheckDefeat()
    {
        foreach (KeyValuePair<Resources, Resource> kvp in resourceDict)
        {
            if (kvp.Value.currentValue <= 0)
            {
                Defeat(kvp.Key);
                return true;
            }
        }
        return false;
    }

    public void Defeat(Resources reason)
    {
        Debug.Log("LOST BECAUSE OF " + reason);
        currentState = gameStateDict[GameStates.DEFEAT];
        currentState.evtStart.Invoke(currentState);
    }
}
