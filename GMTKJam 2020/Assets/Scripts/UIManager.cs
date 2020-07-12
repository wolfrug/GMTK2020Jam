using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class LossText
{
    public Resources reason;
    [Multiline]
    public string text;
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas mainCanvas;
    public Transform playerHandTr;
    public Transform enemyHandTr;
    public Transform playedCardDisplayTr;
    public GameObject UIcardPrefab;
    public GameObject UIcardVisualizerPrefab;
    public Animator DayPassPanelAnimator;
    public Animator LossPanelAnimator;
    public TextMeshProUGUI dayNr;
    public TextMeshProUGUI dayFluffText;

    public TextMeshProUGUI lossDayNr;
    public TextMeshProUGUI lossFluffText;
    public LossText[] lossTexts;

    public Dictionary<Resources, UIResourceObj> resourceObjDict = new Dictionary<Resources, UIResourceObj> { };
    public Dictionary<CardObject, CardObjectUI> playerHand = new Dictionary<CardObject, CardObjectUI> { };
    public Dictionary<CardObject, CardObjectUI> enemyHand = new Dictionary<CardObject, CardObjectUI> { };
    public List<GameObject> allUICardObjects = new List<GameObject> { };
    public List<GameObject> visualizersPlayer = new List<GameObject> { };
    public List<GameObject> visualizersEnemy = new List<GameObject> { };


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

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init()
    {
        foreach (UIResourceObj robj in FindObjectsOfType<UIResourceObj>())
        {
            resourceObjDict.Add(robj.resource.type, robj);
            robj.Init();
        }
    }

    public void StartNewDay()
    {
        playerHandTr.GetComponent<GridLayoutGroup>().enabled = true;
        enemyHandTr.GetComponent<GridLayoutGroup>().enabled = true;
        // Add visualizers
        for (int i = 0; i < GameManager.instance.GetCurrentDay().cardsDrawn / 2; i++)
        {
            CreateVisualizer(CardSide.PLAYER);
            CreateVisualizer(CardSide.OCEAN);
        }
    }
    public void CreateVisualizer(CardSide side)
    {
        if (side == CardSide.PLAYER)
        {
            GameObject newVisualizer = Instantiate(UIcardVisualizerPrefab, playerHandTr);
            visualizersPlayer.Add(newVisualizer);
        }
        else
        {
            GameObject newVisualizer = Instantiate(UIcardVisualizerPrefab, enemyHandTr);
            visualizersEnemy.Add(newVisualizer);
        }
    }

    public void AddCardToHand(CardSide side, CardObject card)
    {
        GameObject UICard = Instantiate(UIcardPrefab, side == card.dataPlayer.side_ ? playerHandTr : enemyHandTr);
        allUICardObjects.Add(UICard);
        CardObjectUI spawnedCard = UICard.GetComponent<CardObjectUI>();
        spawnedCard.UpdateEnemyCard(card.dataEnemy);
        spawnedCard.UpdatePlayerCard(card.dataPlayer);
        spawnedCard.Init();
        spawnedCard.UpdateCard(side);
        spawnedCard.ShowDescription(null);
        if (side == card.dataPlayer.side_)
        {
            playerHand.Add(card, spawnedCard);
            Debug.Log("Visualizer 0: " + visualizersPlayer[0], visualizersPlayer[0]);
            Destroy(visualizersPlayer[0]);
            visualizersPlayer.RemoveAt(0);
            Debug.Log("Destroying player visualizer");
        }
        else
        {
            enemyHand.Add(card, spawnedCard);
            Destroy(visualizersEnemy[0]);
            visualizersEnemy.RemoveAt(0);
            Debug.Log("Destroying enemy visualizer");
        }
    }

    public void UpdateResource(Resources target)
    {
        UIResourceObj resourceObj = null;
        resourceObjDict.TryGetValue(target, out resourceObj);
        if (resourceObj != null)
        {
            resourceObj.UpdateObj();
        }
    }
    public void UpdateAllResources()
    {
        foreach (KeyValuePair<Resources, UIResourceObj> kvp in resourceObjDict)
        {
            kvp.Value.UpdateObj();
        }
    }

    public IEnumerator UseCard(CardObject card)
    {
        playerHandTr.GetComponent<GridLayoutGroup>().enabled = false;
        enemyHandTr.GetComponent<GridLayoutGroup>().enabled = false;
        CardObjectUI selectedCard = null;
        playerHand.TryGetValue(card, out selectedCard);
        if (selectedCard != null)
        {
            playerHand.Remove(card);
            Destroy(selectedCard.gameObject);
            CreateVisualizer(CardSide.PLAYER);
            yield return card.StartCoroutine(TeleportCard(card, CardSide.PLAYER));
        }
        else
        {
            enemyHand.TryGetValue(card, out selectedCard);
            if (selectedCard != null)
            {
                enemyHand.Remove(card);
                Destroy(selectedCard.gameObject);
                CreateVisualizer(CardSide.OCEAN);
                yield return card.StartCoroutine(TeleportCard(card, CardSide.OCEAN));
            }
        }
    }

    IEnumerator TeleportCard(CardObject card, CardSide side)
    {
        float timeToPlay = 0f;
        if (side == CardSide.OCEAN)
        {
            foreach (CardEffect effect in card.dataEnemy.effects)
            {
                timeToPlay += effect.effectTime;
            }
        }
        else
        {
            foreach (CardEffect effect in card.dataEnemy.effects)
            {
                timeToPlay += effect.effectTime;
            }
        }
        //card.transform.SetParent(playedCardDisplayTr, true);
        card.cardAnimator.SetBool("invisible", true);
        yield return new WaitForSeconds(timeToPlay / 4f);
        card.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        card.transform.position = playedCardDisplayTr.transform.position;
        card.cardAnimator.SetBool("invisible", false);
        card.ShowDescription(null);
        UIManager.instance.UpdateAllResources();
        yield return new WaitForSeconds(timeToPlay / 2f);
        card.cardAnimator.SetBool("invisible", true);
        //Destroy(card, 1f);
    }

    public void ClearHands()
    {
        foreach (KeyValuePair<CardObject, CardObjectUI> kvp in playerHand)
        {
            Destroy(kvp.Value.gameObject);
        }
        playerHand.Clear();
        foreach (KeyValuePair<CardObject, CardObjectUI> kvp in enemyHand)
        {
            Destroy(kvp.Value.gameObject);
        }
        enemyHand.Clear();
        foreach (GameObject vis in visualizersPlayer)
        {
            Destroy(vis);
        }
        visualizersPlayer.Clear();
        foreach (GameObject vis in visualizersEnemy)
        {
            Destroy(vis);
        }
        visualizersEnemy.Clear();
        foreach (GameObject crd in allUICardObjects)
        {
            Destroy(crd);
        }
        allUICardObjects.Clear();

    }

    public void SwitchDay()
    {
        ClearHands();
        DayPassPanelAnimator.SetBool("visible", true);
        dayNr.text = GameManager.instance.dayCount.ToString();
        dayFluffText.text = GameManager.instance.GetCurrentDay().fluffText;
    }
    public void FinishSwitchingDay()
    {
        DayPassPanelAnimator.SetBool("visible", false);
        GameManager.instance.NextState();
    }

    public void DisplayLoseScren(Resources reason)
    {
        LossPanelAnimator.SetBool("visible", true);
        lossDayNr.text = GameManager.instance.dayCount.ToString();
        foreach (LossText txt in lossTexts)
        {
            if (txt.reason == reason)
            {
                lossFluffText.text = txt.text;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
