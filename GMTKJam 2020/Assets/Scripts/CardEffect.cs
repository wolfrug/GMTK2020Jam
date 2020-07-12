using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Data", menuName = "Card Effect Data", order = 1)]
public class CardEffect : ScriptableObjectBase
{
    public Resources affectedResource;
    public int change;
    public float effectTime = 1f;
    public ObjectSound soundEffect;

    public virtual void PlayEffect()
    { // basic effect -> change resource
        GameManager.instance.AddResource(affectedResource, change);
    }
    public virtual ObjectSound[] GetSounds
    {
        get
        {
            List<ObjectSound> returnList = new List<ObjectSound> { };
            returnList.Add(soundEffect);
            return returnList.ToArray();
        }
    }



}
