using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

[System.Serializable]
public class CoinPositions
{
    public Transform transform;
    public GameObject holdingOb;
    public int coinPrefabCode;

    public CoinPositions(Transform transform, int coinPrefabCode)
    {
        this.transform = transform;
        this.coinPrefabCode = coinPrefabCode;
        holdingOb = null;
    }
}

[System.Serializable]
public class CoinPrefabSet
{
    public GameObject coinPrefab;
    public Transform coinParent;
}

public class CoinGenerator : MonoBehaviourPunCallbacks
{
    [SerializeField] List<CoinPrefabSet> coinPrefabSets = new List<CoinPrefabSet>();
    List<CoinPositions> coinTransforms = new List<CoinPositions>();
    [SerializeField]float generateInterval;
    [SerializeField] int maxCoinNumber;
    float timer;

    private void Start()
    {
        timer = 0;
        for (int i = 0; i < coinPrefabSets.Count; i++)
        {
            foreach (Transform child in coinPrefabSets[i].coinParent)
            {
                child.gameObject.SetActive(false);
                coinTransforms.Add(new CoinPositions(child, i));
            }
        }
    }

    private void Update()
    {
        if (!InputManager.instance.IsAttractive)
        {
            return;
        }
        
        if (PhotonNetwork.IsMasterClient)
        {
            UpdateMethod();
        }
    }

    private void UpdateMethod()
    {
        timer += Time.deltaTime;
        if (timer > generateInterval)
        {
            // ステージにあるコインの数が最大値を超えた場合、リターン
            if (NowCoinNumber() >= maxCoinNumber)
            {
                return;
            }

            timer = 0;

            int transformCode;
            // 念のため、実行回数を制限する。
            int count = 0;

            while (true)
            {
                transformCode = Random.Range(0, coinTransforms.Count);

                if (coinTransforms[transformCode].holdingOb == null || count > 9999)
                {
                    break;
                }
                count++;
            }

            GameObject nowPrefab = coinPrefabSets[coinTransforms[transformCode].coinPrefabCode].coinPrefab;

            GameObject nowOb = PhotonNetwork.Instantiate(nowPrefab.name,
                coinTransforms[transformCode].transform.position,
                coinTransforms[transformCode].transform.rotation);
            coinTransforms[transformCode].holdingOb = nowOb;
        }
    }

    private int NowCoinNumber()
    {
        return coinTransforms.Count(x => x.holdingOb != null);
    }
}
