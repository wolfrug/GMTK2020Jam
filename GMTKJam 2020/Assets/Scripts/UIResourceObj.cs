using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIResourceObj : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI nameText;
    public Image icon;
    public ResourceData resource;
    public Animator animator;
    // Start is called before the first frame update
    public void Init()
    {
        icon.sprite = resource.icon;
        UpdateObj();
    }

    public void UpdateObj()
    {
        text.text = GameManager.instance.GetResource(resource.type).currentValue.ToString();
        nameText.text = resource.name_ + ":";
    }
    // Update is called once per frame
    void Update()
    {

    }
}
