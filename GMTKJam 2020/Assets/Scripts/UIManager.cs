using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas mainCanvas;
    public Transform playerHandTr;
    public Transform enemyHandTr;
    public GameObject UIcardPrefab;
    public GameObject UIcardVisualizerPrefab;
    public Animator DayPassPanelAnimator;
    public TextMeshProUGUI dayNr;
    public TextMeshProUGUI dayFluffText;

    public Dictionary<Resources, UIResourceObj> resourceObjDict = new Dictionary<Resources, UIResourceObj> { };
    public Dictionary<CardObject, CardObjectUI> playerHand = new Dictionary<CardObject, CardObjectUI> { };
    public Dictionary<CardObject, CardObjectUI> enemyHand = new Dictionary<CardObject, CardObjectUI> { };
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
        CardObjectUI spawnedCard = UICard.GetComponent<CardObjectUI>();
        spawnedCard.UpdateEnemyCard(card.dataEnemy);
        spawnedCard.UpdatePlayerCard(card.dataPlayer);
        spawnedCard.Init();
        spawnedCard.UpdateCard(side);
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

    public void UseCard(CardObject card)
    {
        CardObjectUI selectedCard = null;
        playerHand.TryGetValue(card, out selectedCard);
        if (selectedCard != null)
        {
            Destroy(selectedCard.gameObject);
            playerHand.Remove(card);
            CreateVisualizer(CardSide.PLAYER);
        }
        else
        {
            enemyHand.TryGetValue(card, out selectedCard);
            if (selectedCard != null)
            {
                Destroy(selectedCard.gameObject);
                enemyHand.Remove(card);
                CreateVisualizer(CardSide.OCEAN);
            }
        }
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

    // Update is called once per frame
    void Update()
    {

    }
}
