using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public PacmanMove movement;
   

    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "ghost")
        {
            movement.enabled = false;
            FindObjectOfType<GameManager>().EndGame();
        }
    }*/
}
