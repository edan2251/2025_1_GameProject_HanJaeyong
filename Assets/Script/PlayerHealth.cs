using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxLives = 3;
    public int currentLives = 1;

    public float invincibleTime = 1.0f;
    public bool isInvincible = false;

    // Start is called before the first frame update
    void Start()
    {
        currentLives = maxLives;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Missile"))
        {
            currentLives--;
            Destroy(other.gameObject);

            if(currentLives <= 0)
            {
                GameOver();
            }


        }
    }

    public void GameOver()
    {
        gameObject.SetActive(false);
        Invoke("RestartGame", 3.0f);        //몇초 후에 "" 안의 함수 발동
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene() .name);
    }
}
