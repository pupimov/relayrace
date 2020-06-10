using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlock : MonoBehaviour
{
    public GameCore core;

    void Start()
    {
        //bounds = GetComponent<Collider2D>().bounds;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        //if (other.tag == "Player")
        //{
        //    UnitCore unit = other.GetComponent<UnitCore>();


//            Destroy(other.gameObject);
  //          player.ChangeLine(newLap);
    //    }
    }

    void Update()
    {
        
    }
}
