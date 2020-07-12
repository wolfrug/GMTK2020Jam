using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Data", menuName = "Resource Data", order = 1)]
public class ResourceData : ScriptableObjectBase
{
    public Resources type;
    public int startValue = 10;
    public int maxValue = 99;
    public Sprite icon;
    public Sprite cardIcon;

}
