using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;


public class WcketKeeping : MonoBehaviour
{
    Animator animator;
    BallCollision ballCollision;
    NavMeshAgent navMeshAgent;
    Vector3 initialPos;
    FieldController fieldContro;
    public Transform wicketkeeperhand;
    public Transform keeperNeck;
    public Transform bowler;
    public GameObject ball;
    public bool isWithKeeper = false;
    public bool caught = false;
    public float nearDist = 10f;
    public float endSceneTime = 6f;
    public int lifeLeft = 0;
    public float distBtwWicketAndKeeper = 9.72f;
    public float distKeeperBall = 5f;
    bool areRunning = false;
    public float ballForce = 10f;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lifeLeft = PlayerPrefs.GetInt("life");
        initialPos = ball.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        ballCollision = GameObject.FindObjectOfType<BallCollision>();
        fieldContro = GameObject.FindObjectOfType<FieldController>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        

        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("Ready");
        }

        //if(isHitBat)
        //{
        //    if(new Vector2(ball.transform.position.x - transform.position.x, ball.transform.position.z - ball.transform.position.z).magnitude < nearDist)
        //    {
        //        //navMeshAgent
        //    }

        //    else
        //    {
        //        //run near wicket
        //    }
        //ballCollision.HitKeeper()}

        if(caught)
        {
            GameObject.FindGameObjectWithTag("Ball").transform.position = wicketkeeperhand.transform.position;
            transform.LookAt(bowler.transform);
        }

        if(caught == false)
        {
            transform.LookAt(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
        }

        if ((ball.transform.position.z >= distBtwWicketAndKeeper && ballCollision.HitBat() == false && caught == false) || ((Vector3.Distance(transform.position, new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z) ) < distKeeperBall) && ballCollision.ThrownToKeeper()))
        {
            animator.SetFloat("x", ball.transform.position.x);
            animator.SetFloat("y", ball.transform.position.y - 0.5f);
            //navMeshAgent.enabled = true;
            GetComponent<Animator>().SetTrigger("KeeperCatchAnimation");
            //navMeshAgent.SetDestination(new Vector3(transform.position.x + ball.transform.position.x, transform.position.y, transform.position.z));
        }

        
       
    }



    IEnumerator RestartScene()
    {
        yield return new WaitForSeconds(endSceneTime);
        //if (ballCollision.HitBat() == false)
        //{
        //    lifeLeft--;
        //}
        //PlayerPrefs.SetInt("life", lifeLeft);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ball")
        {
            if (areRunning == false)
            {
                //ball.GetComponent<Rigidbody>().AddForce((wicketkeeperhand.transform.position - ball.transform.position).normalized * ballForce, ForceMode.VelocityChange);
                StartCoroutine(RestartScene());
            }

            //GetComponent<Animator>().SetTrigger("KeeperCatchAnimation");

            //else
            //{
            //    //perform runout procedure;
            //}
            ////StartCoroutine which is in ballThrow

        }
    }

    public void CatchBall()
    {
        caught = true;
        ball.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        ball.GetComponent<Rigidbody>().useGravity = false;
        ball.transform.position = wicketkeeperhand.transform.position;
        ball.transform.position = wicketkeeperhand.transform.position;
        ball.transform.SetParent(wicketkeeperhand.transform);
    }

    public bool WKCaught()
    {
        return caught;
    }

}
