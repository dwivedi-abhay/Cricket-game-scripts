using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BallCollision : MonoBehaviour
{
    public float nonLoftedCollisionForce = 10f;
    public float loftedCollisionForce = 10f;
    public float endSceneTime = 3f;
    ShotContro shotContro;
    public bool isLoft = false;
    bool hitBoundary = false;
    public float y = -0.1f;
    int groundBounce = 0;
    float collisionForce;
    int batColCount = 0;
    public bool hitbat = false;
    public bool hitFielder = false;
    public bool hitKeeper = false;
    bool thrownToKeeper = false;
    int lifeLeft = 0;
    int lastScore = 0;
    int scoreAdded = 0;
    public GameObject runsCanvas;
    public TextMeshProUGUI addedScore;
    public GameObject outCanvas;
    public ParticleSystem ballTrail;

    // Start is called before the first frame update
    void Start()
    {
        shotContro = GameObject.FindObjectOfType<ShotContro>();
        lifeLeft = PlayerPrefs.GetInt("life");
    }

    // Update is called once per frame
    void Update()
    {
        if(isLoft == true)
        {
            y = 1f;
        }
    }

    IEnumerator RestartScene()
    {
        
        yield return new WaitForSeconds(endSceneTime);
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bat")
        {
            hitbat = true;
            batColCount = 1;
            var x = shotContro.DirectionCheckerX();
            var z = shotContro.DirectionCheckerY();
            if (x == 0 && z == 0)
            {
                collisionForce = 0f;
            }
            else
            {
                if (y == 1)
                {
                    collisionForce = loftedCollisionForce;
                }
                else
                {
                    collisionForce = nonLoftedCollisionForce;
                }
            }
            Vector3 forceDirection = new Vector3(x, y, z).normalized;
            gameObject.GetComponent<Rigidbody>().AddForce(forceDirection * collisionForce);
        }

        if(collision.gameObject.tag == "Boundary")
        {
            hitBoundary = true;
        }

        if (collision.gameObject.tag == "Ground")
        {
            groundBounce++;
            CountBounce();
        }




        //if(collision.gameObject.tag == "Wicket Keeper")
        //{
        //    gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
        //    gameObject.transform.SetParent(collision.transform);
        //    gameObject.GetComponent<Rigidbody>().useGravity = false;
        //    hitKeeper = true;
        //}

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Wicket Keeper")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().useGravity = false;
            hitKeeper = true;
        }

        if (other.gameObject.tag == "Fielder")
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            ballTrail.Stop();
            GetComponent<Rigidbody>().useGravity = false;
            hitFielder = true;
            if (groundBounce <= 1)
            {
                GetComponent<Rigidbody>().AddForce((other.transform.position - transform.position), ForceMode.VelocityChange);
                outCanvas.SetActive(true);
                lifeLeft = PlayerPrefs.GetInt("life");
                lifeLeft++;
                PlayerPrefs.SetInt("life", lifeLeft);
                StartCoroutine(RestartScene());
            }
        }

        if (other.gameObject.tag == "Wicket")
        {
            other.gameObject.GetComponent<Animator>().SetTrigger("BallHit");
            outCanvas.SetActive(true);
            lifeLeft = PlayerPrefs.GetInt("life");
            lifeLeft++;
            PlayerPrefs.SetInt("life", lifeLeft);
            StartCoroutine(RestartScene());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Fielder")
        {
            thrownToKeeper = true;
            ballTrail.Play();
            runsCanvas.SetActive(true);
            if(other.gameObject.transform.position.magnitude >= 45f)
            {
                lastScore = PlayerPrefs.GetInt("lastScore");
                lastScore = lastScore + 2;
                PlayerPrefs.SetInt("lastScore", lastScore);
                scoreAdded = 2;
            }

            else if(other.gameObject.transform.position.magnitude >= 30f && other.gameObject.transform.position.magnitude < 45f)
            {
                lastScore = PlayerPrefs.GetInt("lastScore");
                lastScore = lastScore + 1;
                PlayerPrefs.SetInt("lastScore", lastScore);
                scoreAdded = 1;
            }

            addedScore.text = scoreAdded.ToString();
        }
    }


    public int CountBounce()
    {
        
        return groundBounce;
    }

    public void IsLoft(bool isLofted)
    {
        isLoft = isLofted;
    }


    public int BatCollisionCount()
    {
        return batColCount;
    }

    public bool HitBat()
    {
        return hitbat;
    }

    public bool HitFielder()
    {
        return hitFielder;
    }

    public bool HitKeeper()
    {
        return hitKeeper;
    }

    public bool ThrownToKeeper()
    {
        return thrownToKeeper;
    }

    public bool HitBoundary()
    {
        return hitBoundary;
    }

    public int ScoreAdded()
    {
        return scoreAdded;
    }
}
