using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {
    [SerializeField] Transform spawnPoint;
    [SerializeField] string[] Colors;
    [SerializeField] string DefaultColor;


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.CompareTag("Player"))
        {

            // TODO reset platforms
            col.transform.position = spawnPoint.position;
        }
    }

}
