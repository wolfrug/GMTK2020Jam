using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas mainCanvas;
    public Transform playerHandTr;
    public Transform enemyHandTr;
    public GameObject UIcardPrefab;

    public Dictionary<Resources, UIResourceObj> resourceObjDict = new Dictionary<Resources, UIResourceObj> { };
    public List<CardObjectUI> playerHand = new List<CardObjectUI> { };
    public List<CardObjectUI> enemyHand = new List<CardObjectUI> { };

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
        foreach (UIResourceObj robj in FindObjectsOfType<UIResourceObj>())
        {
            resourceObjDict.Add(robj.resource.type, robj);
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
        if (side == CardSide.PLAYER)
        {
            playerHand.Add(spawnedCard);
        }
        else
        {
            enemyHand.Add(spawnedCard);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
