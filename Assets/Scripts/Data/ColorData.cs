using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "MyScript/CreateColorData")]
public class ColorData : ScriptableObject
{
    public int ID;
    public string colorName;
    public Color[] colors = new Color[2];
}
