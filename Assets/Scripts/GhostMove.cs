using UnityEngine;
using System.Collections;

public class GhostMove : MonoBehaviour
{
	public Transform[] waypoints;
	int cur = 0;

	public float speed = 0.3f;

	void FixedUpdate()
	{
		if (transform.position == waypoints[cur].position)
		{
			cur = (cur + 1) % waypoints.Length;
			
		}
		else
        {
			Vector2 p = Vector2.MoveTowards(transform.position, waypoints[cur].position, speed);
			GetComponent<Rigidbody2D>().MovePosition(p);
		}

		// Animation
		Vector2 dir = waypoints[cur].position - transform.position;
		GetComponent<Animator>().SetFloat("DirX", dir.x);
		GetComponent<Animator>().SetFloat("DirY", dir.y);
	}

	void OnTriggerEnter2D(Collider2D co)
	{
		if (co.name == "pacman")
        {
			if (GameManager.getLive() > 0)
            {
				GameManager.setLive();
			}
			else
			{
				Destroy(co.gameObject);
				FindObjectOfType<GameManager>().EndGame();
			}
		}
		
	}
}