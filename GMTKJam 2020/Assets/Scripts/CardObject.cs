using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardObject : MonoBehaviour
{
    public CardBase dataPlayer;
    public CardBase dataEnemy;
    public GenericDragAndDrop dragScript;
    public Animator cardAnimator;
    public Image playerSide;
    public Image enemySide;
    // Start is called before the first frame update
    void Start()
    {
        dragScript.rightClick.AddListener(FlipCard);
        playerSide.sprite = dataPlayer.image_;
        enemySide.sprite = dataEnemy.image_;
    }

    void FlipCard(UnityEngine.EventSystems.PointerEventData data)
    {
        Debug.Log("Flipping card");
        cardAnimator.SetBool("flipped", !cardAnimator.GetBool("flipped"));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
