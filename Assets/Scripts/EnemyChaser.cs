using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaser : MonoBehaviour
{
    public float speed;
    private GameObject player;

    public bool chase = false;
    public Transform startPoint;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) 
        {
            return;
        }
        if (chase == true)
        {
            ChaseYou();
        }
        else 
        {
            ReturnStartPoint();
        }
        Flip();
    }

    void ChaseYou() 
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    void Flip() 
    {
        if (transform.position.x > player.transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else 
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void ReturnStartPoint() 
    {
        transform.position = Vector2.MoveTowards(transform.position, startPoint.position, speed * Time.deltaTime);
    }
}
