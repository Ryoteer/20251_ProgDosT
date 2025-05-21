using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBehaviour>())
        {
            GameManager.Instance.ActualCheckpoint = transform.position;
            _collider.enabled = false;
        }
    }
}
