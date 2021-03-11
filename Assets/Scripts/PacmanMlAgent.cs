using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Timers;

public class PacmanMlAgent : Agent
{
    //set di dati dell'Accademy
    EnvironmentParameters defaultParameters;

    //contatori 
    public int score;
    public int live;

    public GameObject scoreText;
    public GameObject liveText;
    public GameObject newText;


    //Rigidbody Pacman
    Rigidbody2D rb2d;

    //Pacman
    public GameObject pacman;

    //Posizione del Player
    Vector2 pacmanPosition = Vector2.zero;
    Vector2 oldPosition = Vector2.zero;

    //Fantasmini
    public GameObject blinky;
    public GameObject pinky;
    public GameObject inky;
    public GameObject clyde;

    //Pacdot
    [SerializeField] public List<GameObject> pacdot = new List<GameObject>();


    //Posizione dei fantasmini
    Vector2 blinkyPosition;
    Vector2 pinkyPosition;
    Vector2 inkyPosition;
    Vector2 clydePosition;

    //velocità di Pacman
    public float speed = 10f;

    //posizione dei Pacdot
    List<Vector2> pacdotPosition = new List<Vector2>();

    //parametri di gioco
    bool gameHasEnded = false;
    public float restartDelay = 1f;

    //timer
    float timer= 0.5f;
   
 

    //Inizializzazione
    public override void Initialize()
    {
        Debug.Log("inizializzoooo");
        string ScenaAttuale = SceneManager.GetActiveScene().name;

        pacman = GameObject.Find("pacman");
        //posizione Pacman
        pacmanPosition = pacman.transform.position;
        Debug.Log("pacman" + pacmanPosition);

        blinky = GameObject.Find("blinky");
        pinky = GameObject.Find("pinky");
        inky = GameObject.Find("inky");
        clyde = GameObject.Find("clyde");

        //posizione fantasmini
        blinkyPosition = blinky.transform.position;
        pinkyPosition = pinky.transform.position;
        inkyPosition = inky.transform.position;
        clydePosition = clyde.transform.position;

        //inizializzazione Rigidbody Pacman
        rb2d = GameObject.Find("pacman").GetComponent<Rigidbody2D>();
        string str;
        //inizializzazione pacdot
        for (int i = 0; i <= 290; i++)
        {
            str = "pacdot" + i;
            pacdot.Add(GameObject.Find(str));
            pacdotPosition.Add(pacdot[i].transform.position);
        }

        scoreText = GameObject.Find("ScoreText");
        liveText = GameObject.Find("LiveText");
        newText= GameObject.Find("NewText");
        score = 0;
        live = 3;

        //parametri digioco
        bool gameHasEnded = false;
        float restartDelay = 1f;

        defaultParameters = Academy.Instance.EnvironmentParameters;

    }
    //un episodio finisce quando il personaggio raggiunge l'obiettivo finale 
    public override void OnEpisodeBegin()
    {
        Debug.Log("OnEpisodebegin() ");
        //inizializzazione Rigidbody Pacman
        rb2d = GameObject.Find("pacman").GetComponent<Rigidbody2D>();

        pacman = GameObject.Find("pacman");
        //reset posizione di Pacman
        pacman.transform.position = pacmanPosition;

        //reset fantasmi
        if (blinky != null)
        {
            blinky.transform.position = blinkyPosition;
            blinky.GetComponent<GhostMove>().cur = 0;
        }
        else
        {
            blinky= GameObject.Find("blinky");
            blinky.transform.position = blinkyPosition;
        }
        if (pinky != null)
        {
            pinky.transform.position = pinkyPosition;
            pinky.GetComponent<GhostMove>().cur = 0;
        }
        else
        {
            pinky = GameObject.Find("pinky");
            pinky.transform.position = pinkyPosition;
        }
        if (inky != null)
        {
            inky.transform.position = inkyPosition;
            inky.GetComponent<GhostMove>().cur = 0;
        }
        else
        {
            inky = GameObject.Find("inky");
            inky.transform.position= inkyPosition;
        }
        if (clyde != null)
        {
            clyde.transform.position= clydePosition;
            clyde.GetComponent<GhostMove>().cur = 0;
        }
        else
        {
            clyde = GameObject.Find("clyde");
            clyde.transform.position= clydePosition;
        }

       
        SetReward(0);
       

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
        pacman.GetComponent<Animator>().SetFloat("DirX", dir.x);
        pacman.GetComponent<Animator>().SetFloat("DirY", dir.y);

    }
    bool valid(Vector2 dir)
    {
        Vector2 pos = transform.position;
        RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
        return (hit.collider == GameObject.Find("pacman").GetComponent<Collider2D>());
    }
    public override void OnActionReceived(float[] vectorAction) //gestire il movimento
    {

        Vector2 p = Vector2.MoveTowards(transform.position, pacmanPosition, speed);
        rb2d.MovePosition(p);
        if(Mathf.FloorToInt(vectorAction[0]) == 0)
        {
            //AddReward(-0.00003f);
            Debug.Log("sto fermo");
        }
        else if (Mathf.FloorToInt(vectorAction[0]) == 1)
        {
            pacmanPosition = (Vector2)transform.position + Vector2.up;
            //AddReward(0.0002f);
            Debug.Log("vado sopra");
        }
        else if (Mathf.FloorToInt(vectorAction[0]) == 2) //sotto
        {
            pacmanPosition = (Vector2)transform.position - Vector2.up;
            //AddReward(0.0002f);
            Debug.Log("vado sotto");
        }
        else if (Mathf.FloorToInt(vectorAction[0]) == 3)
        {
            pacmanPosition = (Vector2)transform.position - Vector2.right;
            //AddReward(0.0002f);
        }
        else if (Mathf.FloorToInt(vectorAction[0]) == 4)
        {
            pacmanPosition = (Vector2)transform.position + Vector2.right;
            //AddReward(0.0002f);
        }
        Vector2 dir = pacmanPosition - (Vector2)transform.position;
       pacman.GetComponent<Animator>().SetFloat("DirX", dir.x);
       pacman.GetComponent<Animator>().SetFloat("DirY", dir.y);

    }


    void FixedUpdate()
    {
        RequestDecision();
        //this.transform.position = oldPosition;
        timer = timer - Time.deltaTime;
        //Debug.Log("timer"+ timer);
        //Debug.Log("distanza :" + Vector2.Distance(oldPosition, pacmanPosition));
        if (timer < 0.0f)
        {
            Debug.Log("sto in timer");
            AddReward(-0.01f);
           /* if (Vector2.Distance( oldPosition , pacmanPosition) < 2f)
            {
                Debug.Log("sono qui in fixed update ");

                AddReward(-0.01f);
                //Debug.Log("prima: "+oldPosition);
                oldPosition = pacmanPosition;
                //Debug.Log("dopo: " + oldPosition);
                //EndEpisode();
            }*/
            timer = 0.5f;
        }
        
}
public int getLive()
    {
        return this.live;
    }

    public void setLive()
    {
        this.live -= 1;
        this.liveText.GetComponent<Text>().text = "Live: " + this.live;
        this.pacman.GetComponent<Transform>().position = new Vector2(71.3f, 32.5f);
        blinky.GetComponent<Transform>().position = blinkyPosition;
        blinky.GetComponent<GhostMove>().cur = 0;
        pinky.GetComponent<Transform>().position = pinkyPosition;
        pinky.GetComponent<GhostMove>().cur = 0;
        inky.GetComponent<Transform>().position = inkyPosition;
        inky.GetComponent<GhostMove>().cur = 0;
        clyde.GetComponent<Transform>().position = clydePosition;
        clyde.GetComponent<GhostMove>().cur = 0;

    }

    public void setScore()
    {
        this.score += 1;
        this.scoreText.GetComponent<Text>().text = "Score: " + this.score;
        if (this.score == 291)
        {
            AddReward(2f);
            this.newText.GetComponent<Text>().text = "YOU WIN! ! ! !";
            score = 0;
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            EndEpisode();
            Invoke("Restart", restartDelay);
        }

    }
    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            this.newText.GetComponent<Text>().text = "GAME OVER !!!!!!";
            Invoke("Restart", restartDelay);
        }
    }


    void Restart()
    {
        this.live = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    //Osservazioi del player
    public override void CollectObservations(VectorSensor sensor)
    {
        
        sensor.AddObservation(this.transform.position);
        sensor.AddObservation(blinky.transform.position);
        sensor.AddObservation(pinky.transform.position);
        sensor.AddObservation(inky.transform.position);
        sensor.AddObservation(clyde.transform.position);
        for (int i = 0; i <= 290; i++)
        {
            sensor.AddObservation(pacdotPosition[i]);
        }

    }

    void OnTriggerEnter2D(Collider2D co)
    {
         if (co.tag == "ghost")
         {
             if (this.getLive() > 0)
             {
                AddReward(-0.3f);
                Debug.Log("ho perso una vita");
                this.setLive();

            }
             else
             {
                AddReward(-2f);
                Debug.Log("le ho perse tutte");
                this.EndGame();
             }
         }

         if (co.tag == "pacdot")
         {
            timer = 0.5f;
             string name = co.name;
             Destroy(GameObject.Find(name));
             //pacdot.Remove(GameObject.Find(name));
             AddReward(0.005f);
             this.setScore();
            Debug.Log("mangio pacdot");
         }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "mazes")
        {
            Debug.Log("sto sbattendo");
            //AddReward(-0.001f);

        }
    }
}
