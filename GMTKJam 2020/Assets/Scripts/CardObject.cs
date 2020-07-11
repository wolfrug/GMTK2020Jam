using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardObject : MonoBehaviour
{
    public CardBase dataPlayer;
    public GameObject cardDescriptionObj;
    public TextMeshProUGUI cardDescriptionText;
    public bool startOnRandomSide = true;
    public CardSide currentSide;
    public CardBase dataEnemy;
    public GenericDragAndDrop dragScript;
    public Animator cardAnimator;
    public Image playerSide;
    public Image enemySide;
    // Start is called before the first frame update
    public virtual void Start()
    {
        dragScript.rightClick.AddListener(FlipCard);
        dragScript.pointerEntered.AddListener(ShowDescription);
        dragScript.pointerExited.AddListener(HideDescription);
        playerSide.sprite = dataPlayer.image_;
        enemySide.sprite = dataEnemy.image_;
        currentSide = cardAnimator.GetBool("flipped") ? dataEnemy.side_ : dataPlayer.side_;
        if (startOnRandomSide)
        {
            if (Random.Range(0f, 1f) >= 0.5f)
            {
                FlipCard(null);
            }
        }
    }

    void FlipCard(UnityEngine.EventSystems.PointerEventData data)
    {
        Debug.Log("Flipping card");
        cardAnimator.SetBool("flipped", !cardAnimator.GetBool("flipped"));
        currentSide = cardAnimator.GetBool("flipped") ? dataEnemy.side_ : dataPlayer.side_;
    }
    public void ShowDescription(UnityEngine.EventSystems.PointerEventData data)
    {
        cardDescriptionText.text = currentSide == dataEnemy.side_ ? dataEnemy.description_ : dataPlayer.description_;
        cardDescriptionObj.SetActive(true);
    }
    public void HideDescription(UnityEngine.EventSystems.PointerEventData data)
    {
        cardDescriptionObj.SetActive(false);
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
    // Update is called once per frame
    void Update()
    {

    }
}
