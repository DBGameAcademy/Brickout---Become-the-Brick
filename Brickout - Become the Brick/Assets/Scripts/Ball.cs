using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    GameManager gameManager;
    Kid Kid;
    public Vector2 direction;
    public float speed = 5.0f;
    public float maxSpeed = 10f;
    bool isRed = false;
    float redBallSpeedTimer = 10.0f;
    float redBallSpeedT;

    public GameObject particlesPrefab;

    TrailRenderer trailRenderer;
    SpriteRenderer spriteRenderer;
    CircleCollider2D circleCollider;
    GameObject lastObjectHit;

    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        circleCollider = GetComponent<CircleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        
        Kid = gameManager.Kid;
    }

    private void Start()
    {
        speed = Random.Range(speed, maxSpeed);
        lastObjectHit = null;

        //50% of balls should go towards the player last pos
        if(Random.value < 0.5)
        {
            direction = new Vector2(gameManager.Player.transform.position.x - transform.position.x, Mathf.Abs(gameManager.Player.transform.position.y - transform.position.y)).normalized;
            spriteRenderer.color = Color.red;
            isRed = true;
        }
        else
        {

            direction = new Vector2(Random.Range(-7, 7), Random.Range(4, 7)).normalized;
            spriteRenderer.color = new Color(Random.value - 0.5f, Random.value + 0.5f, Random.value + 0.5f);
            isRed = false;
        }
        
        trailRenderer.startColor = spriteRenderer.color;

    }

    private void Update()
    {
        if(gameManager.GameState == GameManager.State.Playing)
        {
            //redball speed increase
            if (isRed)
            {
                redBallSpeedT -= Time.deltaTime;
                if (redBallSpeedT <= 0)
                {
                    redBallSpeedT = 0;
                    speed += 0.4f;
                    redBallSpeedT = redBallSpeedTimer;
                }
            }

            transform.Translate(direction * speed * Time.deltaTime);
            CollisionDetection();
            CheckPosition();
        }
    }

    void CollisionDetection()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, circleCollider.radius, direction, (direction * Time.deltaTime).magnitude);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != circleCollider && hit.transform.gameObject != lastObjectHit)
            {
                //preventig hit on the same object multiple times
                lastObjectHit = hit.transform.gameObject;

                //direction change on collision + increase speed

                direction = Vector2.Reflect(direction * 1.02f, hit.normal);
                
                

                //paddle will always pass a positive value upwards
                if (hit.transform.GetComponent<Paddle>())
                {
                    direction.y = Mathf.Abs(direction.y);

                    if(isRed)
                    {
                        direction = new Vector2(gameManager.Player.transform.position.x - transform.position.x, Mathf.Abs(gameManager.Player.transform.position.y - transform.position.y)).normalized;

                    }

                    Kid.Paddle.OnHit();
                }

                //calling the block on hit function 
                if (hit.transform.GetComponent<Player>())
                {
                    hit.transform.GetComponent<Player>().OnHit();
                }

                Instantiate(particlesPrefab,hit.point, Quaternion.identity);
            }
        }
    }

    void CheckPosition()
    {
        //if the ball goes under the paddle pos kill the ball
        if(transform.position.y < Kid.Paddle.transform.position.y - 1)
        {
            LoseBall();
        }
        if(transform.position.y > 12)
        {
            LoseBall();
        }
    }

    public void LoseBall()
    {
        
        lastObjectHit = null; 

        //reset the paddle current target
        if(Kid.Paddle.currTargetBall == this)
        {
            Kid.Paddle.currTargetBall = null;
        }

        //update the ballspawner balllist withouth this ball and destroy it.
        Kid.BallSpawner.ballList.Remove(this);


        //destroy the gameObject
        Destroy(gameObject);
    }
}
