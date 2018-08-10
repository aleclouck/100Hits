using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class attack : MonoBehaviour
{

    public Animator anim;
    public AudioSource attackSound;
    public int hitRange = 4;
    public int health = 10;
    //private aiHealth enemyHealthScript;

    public Text healthText;
    public Slider healthSlider;

    public Collider attackHitbox;
    //this is a flag to check if the player is in an attacking state
    private bool isAttacking;
    //this flag determines iwhther the player should get health back
    private bool gainHealth;
    //Keeps track of current hit attack combo state
    private int combo;
    //keeps track of the point in time the last swing took place
    private float timeofLastSwing = 0;
    //this is the amount of time allowed between swings in sec to still be a combo attack
    private float timeAllowedBetweenSwings;

    //this will be used to alter the waves
    public GameObject spawner;
    private SpawnEnemies spawnerScript;

    /*
    //for the end game
    public Canvas endCanvas;
    public bool isFinished;
    */


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        isAttacking = false;
        gainHealth = false;
        combo = 0;
        timeAllowedBetweenSwings = 0.5f;
        spawnerScript = spawner.GetComponent<SpawnEnemies>();
    }

    // Update is called once per frame
    void Update()
    {
        if (combo >= 4 || Time.time > (timeofLastSwing + timeAllowedBetweenSwings))
        {
            combo = 0;
            anim.SetInteger("SwingCount", combo);
        }
        if (Input.GetMouseButtonDown(0) && spawnerScript.isPaused == false && spawnerScript.isFinished == false)
        {
            combo++;
            if (combo >= 4)
            {
                combo = 1;
            }
            attacking(attackHitbox);
        }
        

        modifyHealth();


        //if run out of health, game over, game is finished
        if(health == 0)
        {
            spawnerScript.isFinished = true;
        }

    }

    public void modifyHealth()
    {
        //player uses a attack, take away health
        if (isAttacking == true && gainHealth == false)
        {
            health -= 1;       
        }
        else if (isAttacking == true && gainHealth == true)
        {
            health += 3;
        }

        healthSlider.value = health;
        healthText.text = health + " ";
        isAttacking = false;
        gainHealth = false;
        
    }

    public void attacking(Collider col)
    {
        //press button, play animation
        //next step is to set up health and create collison between the player and enemy

        
        anim.SetInteger("SwingCount", combo);
        attackSound.Play();
        isAttacking = true;
        timeofLastSwing = Time.time;
        

        var cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in cols)
        {
            if (c.transform.parent.GetComponent<aiHealth>().EnemyDeath == false)  //death cheack
            {
                Debug.Log(c.name);
                if (c.name == "GiantHitBoxDemon" || c.name == "MiniHitBoxDemon") //checks for hitbox is demon
                {


                    if (c.name == "GiantHitBoxDemon")
                    {
                        c.transform.parent.gameObject.GetComponent<aiHealth>().giantHealth--;
                        c.transform.parent.GetComponent<aiHealth>().Damagetake = true;

                    }
                    else
                    {
                        c.transform.parent.gameObject.GetComponent<aiHealth>().miniHealth--;
                        c.transform.parent.GetComponent<aiHealth>().Damagetake = true;
                    }

                    //if enemy is dead, 
                    if (c.transform.parent.gameObject.GetComponent<aiHealth>().giantHealth <= 0 || c.transform.parent.gameObject.GetComponent<aiHealth>().miniHealth <= 0)
                    {
                        gainHealth = true;
                        spawnerScript.enemiesAlive--;
                        c.transform.parent.GetComponent<aiHealth>().EnemyDeath = true;
                        //Destroy(c.transform.parent.gameObject);
                    }
                }
                else if (c.name == "GiantHitBox" || c.name == "MiniHitBox")//it is a normal enemy hitbox
                {
                    if (c.name == "GiantHitBox")
                    {
                        c.transform.parent.gameObject.GetComponent<aiHealth>().giantHealth--;
                        c.transform.parent.GetComponent<aiHealth>().Damagetake = true;
                    }
                    else
                    {
                        c.transform.parent.gameObject.GetComponent<aiHealth>().miniHealth--;
                        c.transform.parent.GetComponent<aiHealth>().Damagetake = true;
                    }

                    if (c.transform.parent.gameObject.GetComponent<aiHealth>().giantHealth <= 0 || c.transform.parent.gameObject.GetComponent<aiHealth>().miniHealth <= 0)
                    {
                        spawnerScript.enemiesAlive--;
                        c.transform.parent.GetComponent<aiHealth>().EnemyDeath = true;
                        //Destroy(c.transform.parent.gameObject);
                    }
                }
            }
            
        }
    }
}
