using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    bool gameHasEnded = false;
    public static int cont = 0;
    public Text labelText;

    public float restartDelay = 1f;

    public void EndGame()
    {
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            //Debug.Log("GAME OVER");
            FindObjectOfType<Text>().text = "GAME OVER";
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public static void contatore()
    {
        //Debug.Log("Score: " + ++cont);
        //labelText.text = "SCORE: " + ++cont;
        FindObjectOfType<Text>().text = "SCORE: " + ++cont;
        if (cont == 323)
        {
            FindObjectOfType<Text>().text = "YOU WIN";
            cont = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
