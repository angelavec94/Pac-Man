using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public static int cont = 0;
    public static int live = 3;
    public Text labelText;

    public float restartDelay = 1f;

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            //Debug.Log("GAME OVER");
            GameObject.Find("labelText").GetComponent<Text>().text = "GAME OVER";
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        GameManager.live = 3;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static int getLive()
    {
        return live;
    }
    public static void setLive()
    {
        live -= 1;
        GameObject.Find("labelText_live").GetComponent<Text>().text = "Live: " + live;
        GameObject.Find("pacman").GetComponent<Transform>().position = new Vector3(71, 56, 0);

    }

    public static void contatore()
    {
        //Debug.Log("Score: " + ++cont);
        //labelText.text = "SCORE: " + ++cont;
        GameObject.Find("labelText").GetComponent<Text>().text = "Score: " +  ++cont;
        if (cont == 323)
        {
            FindObjectOfType<Text>().text = "YOU WIN";
            cont = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
