﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8f;


    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);
        if (transform.position.y > 8)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            Destroy(this.gameObject);
        }
    }
}
