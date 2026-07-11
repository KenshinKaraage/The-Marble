using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager instance;

    public List<SceneData> sceneDataList = new List<SceneData>();
    public int MaxPlaySceneNum;
    //�}�X�^�[�p
    public int NowSceneNum { set; get; }
    public bool[] IsGoneList;

    public List<PatternData> patternDatas = new List<PatternData>();
    public List<ColorData> colorDatas = new List<ColorData>();

    public List<bool> havePattern = new List<bool>();
    public List<bool> haveColor = new List<bool>();

    public int nowPatternCode;
    public int nowColorCode;

    private void Awake()
    {
        IsGoneList = new bool[sceneDataList.Count];
        patternDatas = patternDatas.OrderBy(x => x.ID).ToList();
        colorDatas = colorDatas.OrderBy(x => x.ID).ToList();

        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public PatternData GetPatternData()
    {
        return patternDatas[nowPatternCode];
    }

    public ColorData GetColorData()
    {
        return colorDatas[nowColorCode];
    }

    public SceneData GetNowSceneData()
    {
        return sceneDataList[(int)PhotonNetwork.CurrentRoom.CustomProperties["NowScene"]];
    }

    public void ResetGoneList()
    {
        for (int i = 0; i < IsGoneList.Length; i++)
        {
            IsGoneList[i] = false;
        }
    }

    public int GetSceneRandomly()
    {
        int notGoneSceneCount = IsGoneList.Count(x => !x);
        if (notGoneSceneCount == 0)
        {
            return -1;
        }

        int index = 0;
        while (true)
        {
            index = Random.Range(0, IsGoneList.Length);
            if (!IsGoneList[index])
            {
                break;
            }
        }

        IsGoneList[index] = true;
        return index;
    }
}
