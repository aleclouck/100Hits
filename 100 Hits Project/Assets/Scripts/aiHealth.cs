using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aiHealth : MonoBehaviour {

    public int giantHealth = 2;
    public int miniHealth = 1;
    public bool Damagetake;
    public bool EnemyDeath = false;
    public float delay = 5f;

    public Animator anim;

    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Damagetake)
        {
            if (EnemyDeath)
            {
                anim.SetTrigger("Dead");
                
                Destroy(gameObject, anim.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
            }
            else
            {
                //print("Damage");
                anim.SetTrigger("Damage");
                Damagetake = false;
            }

        }
	}
}
