using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class FieldController : MonoBehaviour
{
    public GameObject ball;
    public Transform hand;
    NavMeshAgent navMeshAgent;
    Animator animator;
    Vector3 distanceBetweenBallAndFielder;
    Vector3 lastDistBetBallAndFielder;
    Vector3 ballThrowVelocity;
    Vector2 distAlongXZ;
    Vector2 velocityAlongXZ;
    Vector2 ballColGround;
    public bool isInHandOfFielder = false;
    float diff;
    BallCollision ballCollision;
    Boundaries boundaries;
    WcketKeeping wkKeeper;
    bool isHit = false;
    public bool isCaught = false;
    float maxDistance = 15f;
    public float maxDistanceInner = 15f;
    public float ballForce = 10f;
    public float maxDistanceOuter = 30f;
    public float innerVelocity = 7f;
    public float outerVelocity = 10f;
    public float estDist = 65f;
    public float optimumTimeToCatch = 0.1f;
    public float minDistance = 5f;
    int count = 0;
    int count1 = 0;
    //int count2 = 0;
    float endSceneTime = 2f;
    float ballFallTime = 0f;
    bool ballHit = false;
    public Transform keeper;
    int lifeLeft;
    int groundCollision = 1;
    float catchValue = 0f;
    public float innerFieldAngle = 120f;
    public float outerFieldAngle = 80f;
    float fieldAngle = 0f;


    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        animator = GetComponent<Animator>();
        distanceBetweenBallAndFielder = (gameObject.transform.position - ball.transform.position);
        lastDistBetBallAndFielder = (gameObject.transform.position - ball.transform.position);
        boundaries = GameObject.FindObjectOfType<Boundaries>();
        CheckFielderPos();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("Ready");
        }


        if (isInHandOfFielder)
        {
            animator.SetBool("isRunning", false);
        }

        if(navMeshAgent.isActiveAndEnabled)
        {
            if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                animator.SetBool("isRunning", false);
            }
            if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
            {
                navMeshAgent.SetDestination(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
            }
        }
        

        if (boundaries.GoneForBoundary())
        {
            navMeshAgent.enabled = false;
        }
    }



    private void FixedUpdate()
    {
        if (navMeshAgent.isActiveAndEnabled == false)
        {
            gameObject.transform.LookAt(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
        }
        else
        {
            gameObject.transform.LookAt(navMeshAgent.velocity.normalized * 10f + transform.position);
        }
        ballCollision = GameObject.FindObjectOfType<BallCollision>();
        wkKeeper = GameObject.FindObjectOfType<WcketKeeping>();
        isHit = ballCollision.HitBat();
        isInHandOfFielder = ballCollision.HitFielder();
        distanceBetweenBallAndFielder = (new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z) - gameObject.transform.position);
        distAlongXZ = new Vector2(distanceBetweenBallAndFielder.x, distanceBetweenBallAndFielder.z);
        velocityAlongXZ = new Vector2(ball.GetComponent<Rigidbody>().velocity.x, ball.GetComponent<Rigidbody>().velocity.z);
        navMeshAgent.enabled = true;


        if (new Vector2(gameObject.transform.position.x, gameObject.transform.position.z).magnitude < 93f)
        {
            FieldControl();
        }

        if (isInHandOfFielder)
        {
            navMeshAgent.enabled = false;
            animator.SetBool("isRunning", false);

        }



        if (ballHit || isCaught) //ball hit is true when ball is hit by fielder
        {
            //calculate velocity to throw to fielder
            //then throw the ball to the keeper
            gameObject.transform.LookAt(keeper.transform);
            //Invoke("ThrowBallToKeeper", 1f);
        }




    }

    public void ThrowBallToKeeper() //TODO change the projectile angle according to the distance of fielder
    {
        
        float dispBtwBallAndKeeper = (keeper.transform.position - transform.position).magnitude;
        float velocityMag = Mathf.Sqrt(dispBtwBallAndKeeper * Physics.gravity.magnitude / 2);
        ballThrowVelocity = ((keeper.transform.position - transform.position).normalized * (velocityMag / Mathf.Sqrt(2f)) + Vector3.up * (velocityMag / Mathf.Sqrt(2f)));
        ball.transform.SetParent(null);
        isCaught = false;
        ball.GetComponent<Rigidbody>().AddForce(ballThrowVelocity * 1.3f , ForceMode.VelocityChange);
        ball.GetComponent<Rigidbody>().useGravity = true;
        ballHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Fielder")
        {
            float distCompare = Vector3.Distance(transform.position, ball.transform.position) - Vector3.Distance(other.gameObject.transform.position, ball.transform.position);
            if (distCompare <= 0f)
            {
                print("ha bhai tu le le");
                navMeshAgent.velocity = Vector3.zero;
                navMeshAgent.enabled = false;
                GetComponent<CapsuleCollider>().enabled = false;
                animator.SetBool("isRunning", false);
            }
            else
            {
                other.gameObject.GetComponent<NavMeshAgent>().velocity = Vector3.zero;
                other.gameObject.GetComponent<CapsuleCollider>().enabled = false;
                other.gameObject.GetComponent<Animator>().SetBool("isRunning", false);
            }
        }

        if (other.gameObject.tag == "Ball")
        {
            //ball.GetComponent<Rigidbody>().AddForce((hand.transform.position - ball.transform.position).normalized * ballForce, ForceMode.VelocityChange);
            if (ballCollision.CountBounce() <= 1)
            {
                //implement catch animation
                animator.SetFloat("CatchParameter", catchValue);
                animator.SetTrigger("NearBallLofted");
                isCaught = true;
                
            }

            else
            {
                //implement pick and throw animation
                animator.SetTrigger("NearBallGrounded");
                if ((transform.position).magnitude <= 30f)
                {
                    if ((keeper.transform.position - transform.position).magnitude > 15f)
                    {
                        catchValue = 1f;
                    }
                    animator.SetFloat("CloseThrowParameter", catchValue);
                    animator.SetTrigger("CloseThrow");
                }

                else
                {
                    animator.SetTrigger("GoalieThrow");
                }
            }
        }
    }

    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(endSceneTime);
        lifeLeft = PlayerPrefs.GetInt("life");
        lifeLeft--;
        PlayerPrefs.SetInt("life", lifeLeft);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    private float CheckFielderPos()
    {
        if ((transform.position).magnitude < 28f)
        {
            maxDistance = maxDistanceInner;
            navMeshAgent.speed = innerVelocity;
            fieldAngle = innerFieldAngle;
        }

        else
        {
            maxDistance = maxDistanceOuter;
            navMeshAgent.speed = outerVelocity;
            fieldAngle = outerFieldAngle;
        }

        return maxDistance;
    }

    private void FieldControl()
    {
        if (isHit == true && isInHandOfFielder == false && wkKeeper.WKCaught() == false && boundaries.GoneForBoundary() == false)
        {
            groundCollision = ballCollision.CountBounce();
            navMeshAgent.enabled = true;
            //if (Vector2.Angle(distAlongXZ, velocityAlongXZ) <= 190f && Vector2.Angle(distAlongXZ, velocityAlongXZ) >= 170f || Vector2.Distance(new Vector2(ball.transform.position.x, ball.transform.position.z), new Vector2(transform.position.x, transform.position.z)) <= 5f)
            //{
            //    navMeshAgent.velocity = Vector3.zero;
            //    animator.SetBool("isRunning", false);
            //}
            if (Vector2.Angle(distAlongXZ, velocityAlongXZ) >= fieldAngle)
            {
                animator.SetBool("isRunning", true);
                if (groundCollision == 1)
                {
                    if (count1 == 0)
                    {
                        Invoke("ChaseLofted", 0.1f);
                        count1 = 1;
                    }
                }
                else
                {
                    ChaseNonLofted();
                }

            }

            //stops fielder from stacking up
            if (Vector3.Angle(distAlongXZ, velocityAlongXZ) < fieldAngle)
            {
                if (distanceBetweenBallAndFielder.magnitude < maxDistance)
                {
                    navMeshAgent.SetDestination(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
                    navMeshAgent.autoBraking = true;
                    animator.SetBool("isRunning", true);
                }

                else
                {
                    navMeshAgent.enabled = false;
                    animator.SetBool("isRunning", false);

                }
            }

        }
    }

    private void ChaseLofted()
    {
        navMeshAgent.enabled = true;
        velocityAlongXZ = new Vector2(ball.GetComponent<Rigidbody>().velocity.x, ball.GetComponent<Rigidbody>().velocity.z);
        ballFallTime = (Mathf.Abs(ball.GetComponent<Rigidbody>().velocity.y) * 2) / Physics.gravity.magnitude + optimumTimeToCatch;
        ballColGround = new Vector2(ball.GetComponent<Rigidbody>().velocity.x, ball.GetComponent<Rigidbody>().velocity.z) * ballFallTime + new Vector2(ball.transform.position.x, ball.transform.position.z);
        float timeToCatchBall = Mathf.Sqrt(2 * Vector2.Distance(new Vector2(transform.position.x, transform.position.z), ballColGround) / navMeshAgent.acceleration) - optimumTimeToCatch;
        if(timeToCatchBall <= ballFallTime)
        {
            navMeshAgent.SetDestination(new Vector3(ballColGround.x, 0f, ballColGround.y));
            navMeshAgent.autoBraking = true;
        }

        else
        {
            ChaseNonLofted();
        }
        //if(Vector2.Distance(ballColGround, new Vector2(transform.position.x, transform.position.z)) < CheckFielderPos())
        //{
        //    Invoke("ChaseNonLofted", 0.1f);
        //}

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            animator.SetBool("isRunning", false);
        }



    }

    private void ChaseNonLofted()
    {
        navMeshAgent.enabled = true;
        if (transform.position.magnitude <= 30f)
        {
            //if the fielder is at inner circle
            navMeshAgent.SetDestination(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
            navMeshAgent.autoBraking = true; 
        }


        //if fielder at outer circle
        else if (transform.position.magnitude > 30f && count == 0)
        {
            Invoke("ChaseBallOuterCircle", 0.1f);
            count = 1;
        }
        if (Vector3.Distance(transform.position, ball.transform.position) < minDistance)
        {
            navMeshAgent.enabled = true;
            navMeshAgent.SetDestination(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
        }
        if (Vector3.Angle(distAlongXZ, velocityAlongXZ) >= 177f && Vector3.Angle(distAlongXZ, velocityAlongXZ) <= 183f)
        {
            navMeshAgent.SetDestination(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
            navMeshAgent.autoBraking = true;
        }
    }

    private void ChaseBallOuterCircle()
    {
        navMeshAgent.enabled = true;
        velocityAlongXZ = new Vector2(ball.GetComponent<Rigidbody>().velocity.x, ball.GetComponent<Rigidbody>().velocity.z).normalized * estDist + new Vector2(ball.transform.position.x, ball.transform.position.z);
        navMeshAgent.SetDestination(new Vector3(velocityAlongXZ.x, transform.position.y, velocityAlongXZ.y));
        navMeshAgent.autoBraking = true;
        if (navMeshAgent.remainingDistance < navMeshAgent.stoppingDistance)
        {
            navMeshAgent.SetDestination(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
        }

    }


    public void PickBall()
    {
        ballHit = true;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.transform.parent = hand.transform;
        ball.transform.position = hand.transform.position;
        gameObject.transform.LookAt(keeper.transform);


    }

}
