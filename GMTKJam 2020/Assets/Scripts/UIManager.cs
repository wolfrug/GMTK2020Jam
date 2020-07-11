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

    public Animator DayPassPanelAnimator;
    public TextMeshProUGUI dayNr;
    public TextMeshProUGUI dayFluffText;

    public Dictionary<Resources, UIResourceObj> resourceObjDict = new Dictionary<Resources, UIResourceObj> { };
    public Dictionary<CardObject, CardObjectUI> playerHand = new Dictionary<CardObject, CardObjectUI> { };
    public Dictionary<CardObject, CardObjectUI> enemyHand = new Dictionary<CardObject, CardObjectUI> { };

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
        }
        else
        {
            enemyHand.Add(card, spawnedCard);
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
        }
        else
        {
            enemyHand.TryGetValue(card, out selectedCard);
            if (selectedCard != null)
            {
                Destroy(selectedCard.gameObject);
                enemyHand.Remove(card);
            }
        }
    }

    public void ClearHands()
    {
        foreach (KeyValuePair<CardObject, CardObjectUI> kvp in playerHand)
        {
            Destroy(kvp.Value.gameObject);
        }
        foreach (KeyValuePair<CardObject, CardObjectUI> kvp in enemyHand)
        {
            Destroy(kvp.Value.gameObject);
        }
    }

    public void SwitchDay()
    {
        ClearHands();
        StartCoroutine(SwitchDayCR());
    }
    IEnumerator SwitchDayCR()
    {
        DayPassPanelAnimator.SetBool("visible", true);
        dayNr.text = GameManager.instance.dayCount.ToString();
        dayFluffText.text = GameManager.instance.GetCurrentDay().fluffText;
        yield return new WaitForSeconds(4f);
        DayPassPanelAnimator.SetBool("visible", false);
        GameManager.instance.NextState();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
