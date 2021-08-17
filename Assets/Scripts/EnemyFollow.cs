using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform character;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator minorEnemyAnimator;

    void Start()
    {
        character.Find("Character");
        rb = this.GetComponent<Rigidbody2D>();
        minorEnemyAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = character.position - transform.position;
        direction.Normalize();
        movement = direction;

        AnimateSprite();
    }

    private void FixedUpdate()
    {
        MoveCharacter(movement);
    }

    void MoveCharacter(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
    }


    void AnimateSprite()
    {
        minorEnemyAnimator.SetFloat("Horizontal", movement.x);
        minorEnemyAnimator.SetFloat("Vertical", movement.y);
    }
}
