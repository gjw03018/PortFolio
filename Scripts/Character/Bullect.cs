using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullect : MonoBehaviour
{
    public Transform player;
    public float damage;
    public LayerMask layer;
    private void Start()
    {
        Destroy(this.gameObject, 5.0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.layer == 7)
        {
            Debug.Log("Touch");
            collision.GetComponent<PlayObjectClass>().Reaction(damage, player);
            Destroy(this.gameObject);
        }
    }

}
