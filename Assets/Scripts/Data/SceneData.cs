using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MyScript/CreateSceneData")]
public class SceneData : ScriptableObject
{
    public Sprite sprite;
    public string sceneName;
    public string stageName;
    public enum SceneType
    {
        LACE,
        SURVIVAL,
        SCORE,
    }
    public SceneType type;
    public float timeLimit;
    public float visualizeTime;
    public string BGMName;
    [TextArea(0,3)] public string description;
    
}
