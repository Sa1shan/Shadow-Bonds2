using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossKill : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CompareTag("Player"))
        {
            Destroy(player, 4f);
        }
    }
}
