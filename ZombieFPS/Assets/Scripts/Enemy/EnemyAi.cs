using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.SO;
using Assets.Scripts.Player;
using Assets.Scripts.Game;

namespace Assets.Scripts.Enemy
{
    public class EnemyAi : MonoBehaviour
    {
        [Header("Reference")]
        public EnemySO enemySO;
        public Animator animatorEnemy;
        private Health healthScript;
        private GameManager gameManager;
        private NavMeshAgent agent;
        private Transform player;
        [Header("Stat")]
        [SerializeField] private float walkPointRange;
        [SerializeField] private float timeBetweenAttacks;
        [SerializeField] private float sightRange, attackRange;
        [SerializeField] private float timeDeath;
        [Header("In some state")]
        [SerializeField] private bool playerInSightRange, playerInAttackRange;
        [SerializeField] private bool walkPointSet;
        [SerializeField] private bool alreadyAttacked;
        [SerializeField] private bool died;
        [Header("Other")]
        public LayerMask WhatIsGround, WhatIsPlayer;
        private Vector3 walkPoint;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            healthScript = GetComponent<Health>();
            player = GameObject.FindGameObjectWithTag("Player").transform;
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }

        private void Update()
        {
            if(healthScript.health > 0)
            {
                if (died)
                {
                    Resurrection();
                }
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
            else
            {
                if (!died)
                {
                    Death();
                }
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

        private void Resurrection()
        {
            died = false;
            agent.enabled = true;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<CapsuleCollider>().enabled = true;
        }

        private void Death()
        {
            died = true;
            AnimatorController("death", true);
            StartCoroutine(DeathCoroutine());
            agent.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<CapsuleCollider>().enabled = false;
        }

        private IEnumerator DeathCoroutine()
        {
            yield return new WaitForSeconds(timeDeath);
            gameObject.SetActive(false);
            gameManager.ZombieRespawn(gameObject);
        }
    }
}