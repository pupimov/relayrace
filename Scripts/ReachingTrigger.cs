using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReachingTrigger : MonoBehaviour
{
    public bool newLap;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerCore player = other.GetComponent<UnitCore>().Player();
            Destroy(other.gameObject);
            player.ChangeLine(newLap);
        }
    }
}
