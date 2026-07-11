using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScript/CreatePatternData")]
public class PatternData : ScriptableObject
{
    public int ID;
    public string patternName;
    public Texture2D texture;
    public Sprite patternSprite;
}
