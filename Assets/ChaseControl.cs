using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseControl : MonoBehaviour
{
    public EnemyChaser[] enemyArray;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player")) 
        {
            foreach (EnemyChaser enemy in enemyArray) 
            {
                enemy.chase = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("player"))
        {
            foreach (EnemyChaser enemy in enemyArray)
            {
                enemy.chase = false;
            }
        }
    }
}
