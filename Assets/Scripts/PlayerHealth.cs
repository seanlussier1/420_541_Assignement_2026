using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    public int health = 3;
    public static int savedHealth = 3;// use static variable to save the data between scenes
    public Transform respawnPoint;

    public void TakeDamage()
    {
        health--;
        savedHealth = health;

        if (health <= 0)
        {
            savedHealth = 3;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);// resets the level
        }
    }

    private void Start()
    {
        health = savedHealth;// so when we open a new scene the health will sync with the other one.
    }

    private void Update()
    {
        if (transform.position.y < -10f)
        {
            savedHealth = 3;// 
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}