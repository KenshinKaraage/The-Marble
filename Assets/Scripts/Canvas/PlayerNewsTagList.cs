using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNewsTagList : MonoBehaviour
{
    [SerializeField] PlayerNewsTag playerNewsTag;
    List<PlayerNewsTag> playerNewsTags = new List<PlayerNewsTag>();

    [SerializeField] Vector3 firstPosition;
    [SerializeField] Vector3 tagsDistance;

    private void Update()
    {
        int diedTagsIndex = 0;
        //もし、タグが寿命で無くなっていたら、その下のタグは移動するようにする
        for (int i = 0; i < playerNewsTags.Count; i++)
        {
            if (playerNewsTags[i] == null)
            {
                playerNewsTags.RemoveAt(i);
                diedTagsIndex = i;
                break;
            }
        }

        for (int i = diedTagsIndex + 1; i < playerNewsTags.Count; i++)
        {
            playerNewsTags[i].SlideUp();
        }
    }

    public void GeneratePlayerNewsTag(string playerName)
    {
        PlayerNewsTag nowPlayerNewsTag = Instantiate(playerNewsTag, transform);
        switch (DatabaseManager.instance.GetNowSceneData().type)
        {
            case SceneData.SceneType.LACE:
                nowPlayerNewsTag.ChangeTexts(playerName, "GOAL");
                break;
            case SceneData.SceneType.SURVIVAL:
                nowPlayerNewsTag.ChangeTexts(playerName, "×");
                break;
            case SceneData.SceneType.SCORE:
                nowPlayerNewsTag.ChangeTexts(playerName, "+5p");
                break;
            default:
                break;
        }

        nowPlayerNewsTag.transform.localPosition = firstPosition + (tagsDistance) * playerNewsTags.Count;

        playerNewsTags.Add(nowPlayerNewsTag);
    }

}
