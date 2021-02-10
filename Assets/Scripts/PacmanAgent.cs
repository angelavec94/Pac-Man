using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.SceneManagement;

public class PacmanAgent : Agent
{
    //Rigidbody Pacman
    Rigidbody2D rb2d;

    //Pacman
    public GameObject pacman;

    //Posizione del Player
    Vector2 pacmanPosition = Vector2.zero;

    public GameObject blinky;
    public GameObject pinky;
    public GameObject inky;
    public GameObject clyde;

    //Posizione dei fantasmini
    Vector2 blinkyPosition;
    Vector2 pinkyPosition;
    Vector2 inkyPosition;
    Vector2 clydePosition;

    //velocità di Pacman
    public float speed= 0f;

    //contatore
    public int contatore= 0;

    //Inizializzazione
    public override void Initialize()
    {
        speed = 1f;
        pacman = GameObject.Find("pacman");
        //posizione Pacman
        pacmanPosition = pacman.transform.position;

        blinky = GameObject.Find("blinky");
        pinky= GameObject.Find("pinky");
        inky = GameObject.Find("inky");
        clyde = GameObject.Find("clyde");

        //posizione fantasmini
        blinkyPosition = blinky.transform.position;
        pinkyPosition = pinky.transform.position;
        inkyPosition = inky.transform.position;
        clydePosition = clyde.transform.position;

        //inizializzazione Rigidbody Pacman
        rb2d = GameObject.Find("pacman").GetComponent<Rigidbody2D>();
    }

   

    //Osservazioi del player
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(pacman.transform.position);
    }

    
    public override void OnActionReceived(float[] vectorAction) //gestire il movimento
    {
        switch (Mathf.FloorToInt(vectorAction[0]))
        {
            case 0:
                break;
            case 1:
                MoveUp();
                break;
            case 2:
                MoveDown();
                break;
            case 3:
                MoveLeft();
                break;
            case 4:
                MoveRight();
                break;
        }
       
    }
    public void MoveUp()
    {
        rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
    }

    public void MoveDown()
    {
        rb2d.velocity = new Vector2(speed * -1, rb2d.velocity.y);
    }
    public void MoveLeft()
    {
        rb2d.velocity = new Vector2(speed * -1, rb2d.velocity.x);
    }
    public void MoveRight()
    {
        rb2d.velocity = new Vector2(speed, rb2d.velocity.x);
    }
    public override void Heuristic(float[] actionsOut)
    {
        Vector2 p = Vector2.MoveTowards(transform.position, pacmanPosition, speed);
        rb2d.MovePosition(p);
        actionsOut[0] = 0;
        if (Input.GetKey("up"))
        {
            actionsOut[0] = 1;
            pacmanPosition = (Vector2)transform.position + Vector2.up;
        }
            
        if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
        {
            actionsOut[0] = 2;
            pacmanPosition = (Vector2)transform.position - Vector2.up;
        }
            
        if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
        {
            actionsOut[0] = 3;
            pacmanPosition = (Vector2)transform.position - Vector2.right;
        }
            
        if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
        {
            actionsOut[0] = 4;
            pacmanPosition = (Vector2)transform.position + Vector2.right;
        }
        Vector2 dir = pacmanPosition - (Vector2)transform.position;
        GameObject.Find("pacman").GetComponent<Animator>().SetFloat("DirX", dir.x);
        GameObject.Find("pacman").GetComponent<Animator>().SetFloat("DirY", dir.y);


    }
    bool valid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GameObject.Find("pacman").GetComponent<Collider2D>());
    }

    void FixedUpdate()
    {
        RequestDecision();
    }

    void OnTriggerEnter2D(Collider2D co)
    {
        if (co.tag == "ghost")
        {
            if (GameManager.getLive() > 0)
            {
                AddReward(-0.5f);
                GameManager.setLive();
            }
            else
            {
                //Destroy(co.gameObject);
                AddReward(-1f);
                FindObjectOfType<GameManager>().EndGame();
            }
        }

        if(co.tag == "pacdot")
        {
            AddReward(0.1f);
            contatore++;
            if(contatore == 322)
            {
                AddReward(2f);
            }
        }

    }
}
