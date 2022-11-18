using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] private Vector2 attackRange1;
    [SerializeField] private Vector2 attackRange2;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayers;
    private Player_Movement player_movement;
    private Animator animator;
    private float attack_colldown;
    private bool attacking;
    private float attack_anim;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player_movement = GetComponent<Player_Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack_colldown > 0.8f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !player_movement.isMovingY())
            {
                Attack1();
                //attacking = true;
                //player_movement.PlayerStop();
                animator.SetTrigger("attack1");
                //attack_colldown = 0;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1) && !player_movement.isMovingY())
            {
                Attack2();
                //attacking = true;
                //player_movement.PlayerStop();
                animator.SetTrigger("attack2");
                //attack_colldown = 0;
            }
        }

        if (attacking && attack_colldown > 0.3f)
        {
            attacking = false;
            attack_anim = 0;
        }

        attack_colldown += Time.deltaTime;
        attack_anim += Time.deltaTime;
    }

    private void Attack1()
    {
        attacking = true;
        player_movement.PlayerStop();

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange1, 0, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy hitted 1");
        }

        attack_colldown = 0;
    }

    private void Attack2()
    {
        attacking = true;
        player_movement.PlayerStop();

        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange2, 0, enemyLayers);

        attack_colldown = 0;

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy hitted 2");
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireCube(attackPoint.position, attackRange1);
        Gizmos.DrawWireCube(attackPoint.position, attackRange2);
    }

    public bool isAttacking()
    {
        return attacking;
    }
} 
