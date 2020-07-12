using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardObject : MonoBehaviour
{
    public CardBase dataPlayer;
    public CardBase dataEnemy;
    public GameObject cardDescriptionObj;
    public GameObject cardDescLongObj;
    public TextMeshProUGUI cardDescriptionText;
    public TextMeshProUGUI cardDescLongText;
    public bool startOnRandomSide = true;
    public CardSide currentSide;
    public GenericDragAndDrop dragScript;
    public ObjectAudio audioSource;
    public Animator cardAnimator;
    public Image playerSide;
    public Image enemySide;

    public TextMeshProUGUI effectTextPlayer;
    public TextMeshProUGUI effectTextEnemy;
    public TextMeshProUGUI cardNamePlayer;
    public TextMeshProUGUI cardNameEnemy;

    public Image resourceIconPlayer;
    public Image resourceIconEnemy;
    public bool cardPlayed = false;

    public static int debugCounter = 0;
    // Start is called before the first frame update
    public virtual void Start()
    {
        dragScript.rightClick.AddListener(FlipCard);
        dragScript.pointerEntered.AddListener(ShowDescription);
        dragScript.pointerExited.AddListener(HideDescription);
        UpdateCard();
        currentSide = cardAnimator.GetBool("flipped") ? dataEnemy.side_ : dataPlayer.side_;
        if (startOnRandomSide)
        {
            if (Random.Range(0f, 1f) >= 0.5f)
            {
                FlipCard(null);
            }
        }
    }
    public void UpdateCard()
    {
        playerSide.sprite = dataPlayer.image_;
        cardNamePlayer.text = dataPlayer.name_;
        effectTextPlayer.text = dataPlayer.effects[0].change.ToString();
        resourceIconPlayer.sprite = GameManager.instance.GetResource(dataPlayer.effects[0].affectedResource).data.cardIcon;

        enemySide.sprite = dataEnemy.image_;
        cardNameEnemy.text = dataEnemy.name_;
        effectTextEnemy.text = dataEnemy.effects[0].change.ToString();
        resourceIconEnemy.sprite = GameManager.instance.GetResource(dataEnemy.effects[0].affectedResource).data.cardIcon;

        currentSide = cardAnimator.GetBool("flipped") ? dataEnemy.side_ : dataPlayer.side_;
    }

    void FlipCard(UnityEngine.EventSystems.PointerEventData data)
    {
        // Debug.Log("Flipping card");
        HideDescription(null);
        cardAnimator.SetBool("flipped", !cardAnimator.GetBool("flipped"));
        currentSide = cardAnimator.GetBool("flipped") ? dataEnemy.side_ : dataPlayer.side_;
    }
    public void ShowDescription(UnityEngine.EventSystems.PointerEventData data)
    {
        Debug.Log("Showing description for card " + gameObject);
        cardDescriptionText.text = currentSide == dataEnemy.side_ ? dataEnemy.description_ : dataPlayer.description_;
        cardDescriptionObj.SetActive(true);
    }
    public void HideDescription(UnityEngine.EventSystems.PointerEventData data)
    {
        Debug.Log("Hiding description for card " + gameObject);
        cardDescriptionObj.SetActive(false);
    }
    public void ShowLongDescription(bool show)
    {
        cardDescLongText.text = currentSide == dataEnemy.side_ ? dataEnemy.longDescription_ : dataPlayer.longDescription_;
        cardDescriptionObj.SetActive(show);
    }

    public void FlipToPlayerSide(bool flip)
    {
        if (flip)
        {
            if (currentSide != dataPlayer.side_)
            {
                FlipCard(null);
            }
        }
        else if (!flip)
        {
            if (currentSide != dataEnemy.side_)
            {
                FlipCard(null);
            }
        }
    }

    public IEnumerator PlayCard(CardSide sideToPlay = CardSide.NONE) // plays the good or bad effect
    {
        debugCounter++;
        dragScript.interactable = false;
        Debug.Log("Cards played total: " + debugCounter);
        if (sideToPlay == CardSide.NONE) // If set to none, play the appropriate side
        {
            if (currentSide == dataPlayer.side_)
            {
                foreach (CardEffect effect in dataPlayer.effects)
                {
                    effect.PlayEffect();
                    //  audioSource.PlayRandomSoundTypeFromArray(SoundType.PLAY_CARD, effect.GetSounds);
                    yield return new WaitForSeconds(effect.effectTime);
                }
            }
            else
            {
                foreach (CardEffect effect in dataEnemy.effects)
                {
                    effect.PlayEffect();
                    // audioSource.PlayRandomSoundTypeFromArray(SoundType.PLAY_CARD, effect.GetSounds);
                    yield return new WaitForSeconds(effect.effectTime);
                }
            }
        }
        else
        { //play the side wished for
            if (sideToPlay == dataPlayer.side_)
            {
                foreach (CardEffect effect in dataPlayer.effects)
                {
                    effect.PlayEffect();
                    //audioSource.PlayRandomSoundTypeFromArray(SoundType.PLAY_CARD, effect.GetSounds);
                    yield return new WaitForSeconds(effect.effectTime);
                }
            }
            else if (sideToPlay == dataEnemy.side_)
            {
                foreach (CardEffect effect in dataEnemy.effects)
                {
                    effect.PlayEffect();
                    //audioSource.PlayRandomSoundTypeFromArray(SoundType.PLAY_CARD, effect.GetSounds);
                    yield return new WaitForSeconds(effect.effectTime);
                }
            }
        }
        cardPlayed = true;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
