using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSpace : MonoBehaviour
{
    [SerializeField] Spawner spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }

        other.gameObject.SetActive(false);
        other.gameObject.transform.position = spawner.GetRespawnTransform().position;
        other.gameObject.transform.rotation = spawner.GetRespawnTransform().rotation;
        other.gameObject.SetActive(true);
    }
}
