using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    public static GameManager Instance;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private PlayerBehaviour _player;

    public PlayerBehaviour Player
    {
        get { return _player; }
        set { _player = value; }
    }

    private Vector3 _actualCheckpoint;

    public Vector3 ActualCheckpoint
    {
        get { return _actualCheckpoint; }
        set { _actualCheckpoint = value; }
    }

    private Transform[] _aiNodes;

    public Transform[] AINodes
    {
        get { return _aiNodes; }
        set { _aiNodes = value; }
    }

    private NavMeshSurface _surface;

    public NavMeshSurface Surface
    {
        get { return _surface; }
        set { _surface = value; }
    }
}
