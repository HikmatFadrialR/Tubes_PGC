using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject peluru;
    public float KecepatanPeluru;
    Animator anim;
    // public AudioSource playSound;
    // AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        // playSound.Stop();
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)) {
            // playSound.Play();
            GameObject go = Instantiate(peluru) as GameObject;
            Destroy(go, 2f);
            go.transform.position = transform.position;
            go.GetComponent<Rigidbody>().AddForce(transform.forward * KecepatanPeluru);
            anim.SetBool("AK47", true);
        } 
    }
}
