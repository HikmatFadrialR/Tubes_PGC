using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public string tagKejar;
    public float kecepatan;
    GameObject Target;
    Vector3 targetDirection, newDirection;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, kecepatan * Time.deltaTime);

        targetDirection = Target.transform.position - transform.position;
        newDirection = Vector3.RotateTowards(transform.forward, targetDirection, kecepatan * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection); 
        
    }

    private void OnTriggerEnter(Collider other) {
        Target = GameObject.FindWithTag("Player");
        
    }
}