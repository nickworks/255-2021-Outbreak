using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


namespace Foster
{
    public class EnemyAI : MonoBehaviour
    {

        private States.State state;
        static class States
        {
            public class State
            {
                protected EnemyAI enemy;
                virtual public State Update()
                {
                    return null;
                }
                virtual public void OnStart(EnemyAI enemy)
                {
                    this.enemy = enemy;
                }
                virtual public void OnEnd()
                {
                }
            }

            public class Wander : State
            {
                bool transition = false;
                public override State Update()
                {
                    enemy.CarryOutDetection();
                    //behavior
                    if (Time.time > enemy.wanderNextCheck)
                    {
                        enemy.wanderNextCheck = Time.time + enemy.wanderCheckRate;
                        CheckIfIShouldWander();
                    }

                    //transition
                    if (enemy.playerSeen) return new States.Persue();

                    return null;
                }

                public override void OnStart(EnemyAI enemy)
                {
                    transition = false;
                   if( enemy.GetComponent<NavMeshAgent>() != null) enemy.myNavMeshAgent = enemy.GetComponent<NavMeshAgent>();
                    enemy.wanderCheckRate = Random.Range(.01f, .2f);
                    enemy.myTransform = enemy.transform;
                }
                public override void OnEnd()
                {
                    base.OnEnd();
                }

                void CheckIfIShouldWander()
                {
                    if (enemy.myTarget == null && !enemy.isOnRoute && enemy.isNavPaused)
                    {
                        if (RandomWanderTarget(enemy.myTransform.position, enemy.wanderRange, out enemy.wanderTarget))
                        {
                            enemy.myNavMeshAgent.SetDestination(enemy.wanderTarget);
                            enemy.isOnRoute = true;
                            enemy.myNavMeshAgent.speed = 3.5f;
                            transition = true;

                        }
                    }
                }


                bool RandomWanderTarget(Vector3 centre, float range, out Vector3 result)
                {
                    Vector3 randomPoint = centre + Random.insideUnitSphere * enemy.wanderRange;
                    if (NavMesh.SamplePosition(randomPoint, out enemy.navHit, 1.0f, NavMesh.AllAreas))
                    {
                        result = enemy.navHit.position;
                        return true;
                    }
                    else
                    {
                        result = centre;
                        return false;
                    }
                }

            }
            public class Persue : State
            {

                public override State Update()
                {
                    if (enemy.GetComponent<NavMeshAgent>() != null) enemy.myNavMeshAgent = enemy.GetComponent<NavMeshAgent>();
                    if (Time.time > enemy.persueNextCheck)
                    {
                        enemy.persueNextCheck = Time.time + enemy.persueCheckRate;
                        TryToChase();
                    }

                    return null;
                }

                void TryToChase()
                {
                    if(enemy.myTarget != null && enemy.myNavMeshAgent != null && enemy.isNavPaused)
                    {
                        enemy.myNavMeshAgent.SetDestination(enemy.myTarget.position);
                        if(enemy.myNavMeshAgent.remainingDistance > enemy.myNavMeshAgent.stoppingDistance)
                        {
                            enemy.isOnRoute = true;
                        }
                    }
                }
            }

            public class DestinationReached : State
            {

            }

        }

        public bool isOnRoute;
        public bool isNavPaused;
        public PlayerMovement PM;
        public Transform myTarget;

        //Layer Masks
        public LayerMask playerLayer;
        public LayerMask sightLayer;

        private NavMeshAgent myNavMeshAgent;
        private Transform myTransform;

        //Enemy Detection
        private Transform headTransform;
        private float headCheckRate;
        private float headNextCheck;
        public float headDetectRaduis = 8;
        private RaycastHit hitTarget;

        //Destination reached
        private float desitationCheckRate;
        private float desitationNextCheck;

        //Persue
        private float persueCheckRate;
        private float persueNextCheck;

        //Wander
        private float wanderCheckRate;
        private float wanderNextCheck;
        public float wanderRange = 20;
        public NavMeshHit navHit;
        private Vector3 wanderTarget;

        //Enemy Attack
        private Transform attackTarget;
        public float nextAttack;
        public float attackRate = .2f;
        public float attackRange = 6f;
        public float attackDamage = 10f;
        public float closeness = 0f;

        //enemy health 
        public int enemyHealth = 100;


        //???
        bool playerSeen;



        void Start()
        {
            if (GetComponent<NavMeshAgent>() != null)
            {
                myNavMeshAgent = GetComponent<NavMeshAgent>();
            }
            myTransform = transform;

            wanderCheckRate = Random.Range(.01f, .2f);

            if (state == null) SwitchState(new States.Wander());
            if (state != null) SwitchState(state.Update());
        }

        
        void Update()
        {
            CarryOutDetection();
            headCheckRate = Random.Range(.8f, 1.2f);
            if (headTransform == null) headTransform = myTransform;
            if (Time.time > wanderNextCheck)
            {
                wanderNextCheck = Time.time + wanderCheckRate;
            }
        }
        void SwitchState(States.State newState)
        {
            if (newState == null) return;
            if (state != null) state.OnEnd();

            state = newState;
            state.OnStart(this);
        }

        bool CanSeeTarget(Transform potentialTarget)
        {
            if (Physics.Linecast(headTransform.position, potentialTarget.position, out hitTarget, sightLayer))
            {
                if (hitTarget.transform == potentialTarget) return true;
                else return false;
            }
            else return false;
        }    


        void CarryOutDetection()
        {
            if (Time.time > headNextCheck)
            {
                headNextCheck = Time.time + headCheckRate;

                Collider[] colliders = Physics.OverlapSphere(myTransform.position, headDetectRaduis, playerLayer);

                if (colliders.Length > 0)
                {
                    foreach (Collider potentialTargetCollider in colliders)
                    {
                        if (CanSeeTarget(potentialTargetCollider.transform))
                        {
                            playerSeen = true;
                            break;
                        }
                    }
                }

            }
            else myTarget = null;
        }

      

        public void DeductHealth(int healthChange)
        {
            enemyHealth -= healthChange;
            if (enemyHealth <= 0)
            {
                enemyHealth = 0;
                Destroy(gameObject);
             
            }
        }
    }
}