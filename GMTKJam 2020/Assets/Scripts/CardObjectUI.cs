using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardObjectUI : CardObject
{
    // Start is called before the first frame update
    public override void Start()
    {

    }
    public void Init()
    {
        playerSide.sprite = dataPlayer.image_;
        enemySide.sprite = dataEnemy.image_;
        dragScript.pointerEntered.AddListener(ShowDescription);
        dragScript.pointerExited.AddListener(HideDescription);
        currentSide = cardAnimator.GetBool("flipped") ? dataEnemy.side_ : dataPlayer.side_;
    }

    public void UpdatePlayerCard(CardBase data)
    {
        dataPlayer = data;
    }
    public void UpdateEnemyCard(CardBase data)
    {
        dataEnemy = data;
    }
    public void UpdateUICard(CardSide side)
    {
        playerSide.sprite = dataPlayer.image_;
        cardNamePlayer.text = dataPlayer.name_;
        effectTextPlayer.text = dataPlayer.effects[0].change.ToString();
        resourceIconPlayer.sprite = GameManager.instance.GetResource(dataPlayer.effects[0].affectedResource).data.icon;

        enemySide.sprite = dataEnemy.image_;
        cardNameEnemy.text = dataEnemy.name_;
        effectTextPlayer.text = dataEnemy.effects[0].change.ToString();
        resourceIconEnemy.sprite = GameManager.instance.GetResource(dataEnemy.effects[0].affectedResource).data.icon;
        
        FlipToPlayerSide(side == dataPlayer.side_);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
