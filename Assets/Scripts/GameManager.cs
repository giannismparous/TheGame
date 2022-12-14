using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int respawn;

    public static GameManager gameManager { get; private set; }

    public UnitHealth playerHealth = new UnitHealth(100,100);

    void Awake()
    {
        if (gameManager != null && gameManager != this)
        {
            Destroy(this);
        }
        else {
            gameManager = this;
        }
    }

    public void RestartGame() {
        SceneManager.LoadScene(respawn);
    }

}
