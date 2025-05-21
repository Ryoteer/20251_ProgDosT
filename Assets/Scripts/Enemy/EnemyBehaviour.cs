using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EntityBehaviour
{
    private PlayerBehaviour _player;

    protected override void Start()
    {
        base.Start();

        _player = GameManager.Instance.Player;
    }

    public override void TakeDamage(int dmg)
    {
        _actualHP -= dmg;

        if(_actualHP <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log($"{name}: Au. :(");
        }
    }
}
