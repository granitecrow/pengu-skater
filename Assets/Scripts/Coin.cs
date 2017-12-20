using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    private Animator anim;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim.SetTrigger("Spawn");
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            GameManager.Instance.GetCoin();
            anim.SetTrigger("Collected");
        }
    }

}
