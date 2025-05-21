using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBehaviour>())
        {
            GameManager.Instance.Player.ResetPlayer();
        }
    }
}
