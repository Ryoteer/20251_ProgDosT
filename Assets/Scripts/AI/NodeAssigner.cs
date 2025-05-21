using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeAssigner : MonoBehaviour
{
    private void Start()
    {
        Transform[] nodes = GetComponentsInChildren<Transform>();

        GameManager.Instance.AINodes = nodes;
    }
}
