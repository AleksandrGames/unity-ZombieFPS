using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.SO;
using Assets.Scripts.Player;

namespace Assets.Scripts.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        public EnemySO enemySO;
        public NavMeshAgent agent;
        public Animator animatorEnemy;
        public Transform player;
        public LayerMask WhatIsGround, WhatIsPlayer;
        public Vector3 walkPoint;
        public float walkPointRange;
        public float timeBetweenAttacks;
        public float sightRange, attackRange;
        public bool playerInSightRange, playerInAttackRange;
        private bool walkPointSet;
        private bool alreadyAttacked;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        private void Update()
        {
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);
            if (!playerInSightRange && !playerInAttackRange)
            {
                Patroling();
            }
            if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }
        }

        private void Patroling()
        {
            if (!walkPointSet)
            {
                SearchWalkPoint();
            }
            if (walkPointSet)
            {
                agent.SetDestination(walkPoint);
                AnimatorController("walk", true);
            }
            Vector3 distanceToWalkPoint = transform.position - walkPoint;
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
            }
        }

        private void SearchWalkPoint()
        {
            float randomZ = Random.Range(-walkPointRange, walkPointRange);
            float randomX = Random.Range(-walkPointRange, walkPointRange);
            walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
            if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
            {
                walkPointSet = true;
            }
        }

        private void ChasePlayer()
        {
            agent.SetDestination(player.position);
            AnimatorController("walk", true);
        }

        private void AttackPlayer()
        {
            agent.SetDestination(transform.position);
            AnimatorController("walk", false);
            transform.LookAt(player);
            AnimatorController("attack", true);
            if (!alreadyAttacked)
            {
                if (player.gameObject.GetComponent<PlayerCount>() != null)
                {
                    if (player.gameObject.GetComponent<PlayerManager>().armor > 0)
                    {
                        player.gameObject.GetComponent<PlayerCount>().TakeCount(enemySO.damage * 3, "armor");
                    }
                    else
                    {
                        player.gameObject.GetComponent<PlayerCount>().TakeCount(enemySO.damage, "health");
                    }
                }
                alreadyAttacked = true;
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }

        private void ResetAttack()
        {
            alreadyAttacked = false;
        }

        private void AnimatorController(string parametrName, bool enable)
        {
            if(parametrName == "walk" && enable)
            {
                animatorEnemy.SetBool("walk", true);
            }
            else if(parametrName == "walk" && !enable)
            {
                animatorEnemy.SetBool("walk", false);
            }
            if (parametrName == "attack")
            {
                animatorEnemy.SetTrigger("attack");
            }
            if (parametrName == "death")
            {
                animatorEnemy.SetTrigger("death");
            }
        }
    }
}