using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpawnEnemies : MonoBehaviour {

    public GameObject[] enemyNorm;
    public GameObject[] enemyDemon;
    public GameObject[] spawners;
    public int numberOfWaves;
    public int numberOfEnemies;
    public int numberOfDemons;
    public Text waveText;
    public Text numOfEnemiesText;
    public Canvas pauseCanvas;
    public Canvas endCanvas;

    private int currentWave;
    private int numOfDemons;
    public bool isFinished;
    public bool girlReached;
    public int enemiesAlive;
    public bool isPaused;

    private loadScene loadScript;
    CursorLockMode wantedMode;
    

	// Use this for initialization
     void Start () {
        Time.timeScale = (float)1;
        currentWave = 1;
        waveText.text = currentWave + "";
        enemiesAlive = numberOfEnemies + numberOfDemons;
        numOfEnemiesText.text = enemiesAlive + "";
        Spawn(numberOfEnemies, numberOfDemons);
        isFinished = false;
        girlReached = false;
        endCanvas.gameObject.SetActive(false);
        pauseCanvas.gameObject.SetActive(false);
        isPaused = false;
        wantedMode = CursorLockMode.Locked;
        Cursor.lockState = wantedMode;
        // Cursor.visible = (CursorLockMode.Locked != wantedMode);

    }

    void Update()
    {
        //make sure the cursor is on the correct wantedMode
        Cursor.lockState = wantedMode;

        numOfEnemiesText.text = enemiesAlive + "";
        //continually check if enemies r still alive
        //if it is zero, moveo onto the next wave, increment current wave
        //only do this while the currentWave hasnt reached the desired number of waves
        if (currentWave < numberOfWaves)
        {
            //if all the enemies are destroyed move onto the next wave
            if (enemiesAlive == 0)
            {
                currentWave++;
                waveText.text = currentWave + "";
                enemiesAlive = numberOfEnemies + numberOfDemons;        
                Spawn(numberOfEnemies, numberOfDemons);
            }
           
        }
        else //once the current wave has reached the desire wave and all enemies are destroyed, spawn the girl
        {
            if (enemiesAlive == 0 && isFinished == false)
            {            
                isFinished = true;
                waveText.gameObject.SetActive(false);
                numOfEnemiesText.gameObject.SetActive(false);
                endCanvas.gameObject.SetActive(true);
                Cursor.visible = true;
                Time.timeScale = 0;
                wantedMode = CursorLockMode.None;
            }
        }

        //if the game is finished but the enemies are still alive, means player died
        if(isFinished == true && enemiesAlive != 0)
        {
            waveText.gameObject.SetActive(false);
            numOfEnemiesText.gameObject.SetActive(false);
            endCanvas.gameObject.SetActive(true);
            Cursor.visible = true;
            Time.timeScale = 0;
            wantedMode = CursorLockMode.None;
        }

        //press esc key to pause game if isFinished = false
        if(Input.GetKeyDown(KeyCode.Escape) && isFinished == false)
        {
                pauseGame(isPaused);
        }

        
    }

    //function to pause game
    //if the game-state is not paused, pause it and change isPaused = true
    //otherwise unpause game and change isPaused = false
    public void pauseGame(bool state)
    {
        if (state == false)
        {
            Time.timeScale = 0;
            pauseCanvas.gameObject.SetActive(true);
            isPaused = true;
            wantedMode = CursorLockMode.None;
        }
        else
        { 
            Time.timeScale = 1;
            pauseCanvas.gameObject.SetActive(false);
            isPaused = false;
            wantedMode = CursorLockMode.Locked;
        }
    }


    //spawn an enemy cube within the area
    //have to put the type of enemy in the enemy array
    //get the enemy in array and instantiate it within a certain location
    public void Spawn(int numOfEnemy, int numOfDem)
    {
        int randX;
        int randZ;
        int randSpawnIndex;
        int randEnemyIndex;
        Vector3 spawnPos;

        //spawn the demonEnemies first, dont want the player getting to fed, so gonna limit at it to 2 per round
        //demon enemies are in in index 2 and 3 of the enemy array
       for(int i = 0; i < (numOfDem); i++)
        {
            // pick a random spawner
            //get spawner's transform coordiantes
            //spawn enemy at spawner location
            randX = Random.Range(0, 4);
            randZ = Random.Range(0, 4);
            randSpawnIndex = Random.Range(0, spawners.Length);
            spawnPos = spawners[randSpawnIndex].transform.position;
            spawnPos.x += randX;
            spawnPos.z -= randZ;

            //choose random enemy index
            randEnemyIndex = Random.Range(0, enemyDemon.Length);

            //now spawn the enemy
            Instantiate(enemyDemon[randEnemyIndex], spawnPos, transform.rotation);
        }


        //spawns the rest of the enemies
        for (int i = 0; i < (numOfEnemy); i++) {
            // get random spawn location within range
            randX = Random.Range(0, 4);
            randZ = Random.Range(0, 4);
            randSpawnIndex = Random.Range(0, spawners.Length); //choose random spawner
            spawnPos = spawners[randSpawnIndex].transform.position;
            spawnPos.x -= randX;
            spawnPos.z += randZ;
            //choose random enemy to spawn
            randEnemyIndex = Random.Range(0, enemyNorm.Length);

            // actually spawns
            Instantiate(enemyNorm[randEnemyIndex], spawnPos, transform.rotation);
        }   
    }



}
