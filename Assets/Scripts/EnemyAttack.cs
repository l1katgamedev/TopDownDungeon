using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public int enemyDamage;
    private int currentEnemyDamage;


    void Start()
    {
        currentEnemyDamage = enemyDamage;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            collision.gameObject.GetComponent<PlayerController>().HurtCharacter(currentEnemyDamage);
            Debug.Log("Hit the PLAYER");
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Character"))
        {
            collision.gameObject.GetComponent<PlayerController>().HurtCharacter(currentEnemyDamage);
            Debug.Log("STYA HITTING HIM");
        }
    }
}
