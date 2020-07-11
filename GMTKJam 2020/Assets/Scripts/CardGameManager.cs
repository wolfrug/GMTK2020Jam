using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class CardGameManager : MonoBehaviour
{
    public static CardGameManager instance;

    public List<CardObject> playerHand = new List<CardObject> { };
    public List<CardObject> enemyHand = new List<CardObject> { };
    public List<CardObject> allCards = new List<CardObject> { };
    public int playerHandMax = 8;
    public int enemyHandMax = 8;
    public GameObject cardPrefab;
    public BoxCollider2D spawnZone;
    public Timer draftTimer;

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
        draftTimer.timerEnded.AddListener((timer) => FinishDraftStateCheck(true));
    }

    public void StartNextDay()
    {
        playerHandMax = GameManager.instance.GetCurrentDay().cardsDrawn / 2;
        enemyHandMax = playerHandMax;
        SpawnCards();
    }

    public void SpawnCards()
    {
        DayData currentDay = GameManager.instance.GetCurrentDay();
        // Delete all old cards
        foreach (CardObject crd in allCards)
        {
            Destroy(crd);
        }
        // Spawn new ones according to the day!
        for (int i = 0; i < currentDay.cardsDrawn; i++)
        {
            CardObject spawnedCard = SpawnCard();
            CardBase enemySide = currentDay.allEnemyCards.RandomElementByWeight(e => e.Value).Key;
            CardBase playerSide = currentDay.allPlayerCards.RandomElementByWeight(e => e.Value).Key;
            spawnedCard.dataEnemy = enemySide;
            spawnedCard.dataPlayer = playerSide;
            allCards.Add(spawnedCard);
            spawnedCard.transform.position = RandomPointInBounds(spawnZone.bounds);
        }
        // star the timer!
        StartTimer();
    }

    void StartTimer()
    {
        DayData currentDay = GameManager.instance.GetCurrentDay();
        draftTimer.InitTimer(currentDay.draftTime);
    }

    CardObject SpawnCard()
    {
        GameObject spawnedCardGO = Instantiate(cardPrefab, spawnZone.transform, true);
        CardObject card = spawnedCardGO.GetComponent<CardObject>();
        return card;
    }

    public void TriggerFlipToPlayerSide(GameObject target)
    {
        CardObject card = target.GetComponent<CardObject>();
        //card.currentSide = card.dataPlayer.side_;
        card.FlipToPlayerSide(true);
        AddCardToPlayerHand(card);
        FinishDraftStateCheck();
    }
    public void TriggerFlipToEnemySide(GameObject target)
    {
        CardObject card = target.GetComponent<CardObject>();
        //card.currentSide = card.dataEnemy.side_;
        card.FlipToPlayerSide(false);
        AddCardToEnemyHand(card);
        FinishDraftStateCheck();
    }

    public void AddCardToPlayerHand(CardObject card)
    {
        if (playerHand.Count < playerHandMax && !playerHand.Contains(card))
        {
            playerHand.Add(card);
            UIManager.instance.AddCardToHand(card.dataPlayer.side_, card);
            card.dragScript.interactable = false;
            card.cardAnimator.SetBool("invisible", true);
        }
    }
    public void AddCardToEnemyHand(CardObject card)
    {
        if (enemyHand.Count < enemyHandMax && !enemyHand.Contains(card))
        {
            enemyHand.Add(card);
            UIManager.instance.AddCardToHand(card.dataEnemy.side_, card);
            card.dragScript.interactable = false;
            card.cardAnimator.SetBool("invisible", true);
        }
    }

    public void FinishDraftStateCheck(bool forceFinish = false)
    {
        if (playerHand.Count == playerHandMax || enemyHand.Count == enemyHandMax || forceFinish)
        {
            foreach (CardObject crd in allCards)
            {
                if (!enemyHand.Contains(crd) && !playerHand.Contains(crd) && enemyHand.Count < enemyHandMax)
                {
                    AddCardToEnemyHand(crd);
                }
                if (!enemyHand.Contains(crd) && !playerHand.Contains(crd) && playerHand.Count < playerHandMax)
                {
                    AddCardToPlayerHand(crd);
                }
            }
            // Woop!
            draftTimer.StopTimer();
            if (GameManager.instance.GameState == GameStates.DRAFT)
            {
                GameManager.instance.NextState();
            };
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }

    public void StartAutomaticGame()
    {
        StartCoroutine(PlayAutomaticCardGame());
    }

    public IEnumerator PlayAutomaticCardGame()
    {
        // First player plays a card, then enemy, then player etc until all is played
        CardObject randomCard;

        for (int i = 0; i < playerHandMax; i++)
        {
            // PLAYER
            randomCard = playerHand[Random.Range(0, playerHand.Count)];
            yield return StartCoroutine(randomCard.PlayCard(CardSide.PLAYER));
            Destroy(randomCard.gameObject);
            playerHand.Remove(randomCard);
            UIManager.instance.UseCard(randomCard);
            // OCEAN
            randomCard = enemyHand[Random.Range(0, enemyHand.Count)];
            yield return StartCoroutine(randomCard.PlayCard(CardSide.OCEAN));
            Destroy(randomCard.gameObject);
            enemyHand.Remove(randomCard);
            UIManager.instance.UseCard(randomCard);
        }
        Debug.LogWarning("FINISHED AUTOMATIC CARD GAME");
        yield return new WaitForSeconds(1f);
        ClearAllCards();
        // Check if we lost
        if (!GameManager.instance.CheckDefeat())
        {
            GameManager.instance.NextState();
        }
    }

    public void ClearAllCards()
    {
        foreach (CardObject card in allCards)
        {
            Destroy(card);
        }
        allCards.Clear();
    }
}
