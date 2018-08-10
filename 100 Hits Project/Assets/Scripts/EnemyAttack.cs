using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;
    public Collider attackHitbox;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void attacking()
    {
        var cols = Physics.OverlapBox(attackHitbox.bounds.center, attackHitbox.bounds.extents, attackHitbox.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in cols)
        {
           // Debug.Log(c.name);
            if (c.name == "HitBox")
            {
                //Destroy(c.transform.parent.gameObject);
                c.transform.parent.GetComponent<attack>().health--;
            }
        }
    }
}