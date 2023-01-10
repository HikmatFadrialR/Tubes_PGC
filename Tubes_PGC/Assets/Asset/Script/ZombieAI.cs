using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAI : MonoBehaviour
{
    Animator anim;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        // anim.SetBool("Z_Idle", true);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnTriggerEnter(Collider other) {
        
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.destination = target.position;
        anim.SetBool("isRun", true);
    }
}
