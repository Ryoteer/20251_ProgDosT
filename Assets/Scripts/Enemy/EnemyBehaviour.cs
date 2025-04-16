using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : EntityBehaviour
{
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
