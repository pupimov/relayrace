using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCan : UnitCore
{
    public GameObject oil;
    private GameObject oilTemp;

    protected override void Start()
    {
        base.Start();
        oilTemp = oil;
    }

    protected override void OnSpell()
    {
        if (oil == null)
            oil = oilTemp;

        oil = Instantiate(oil, 
                          new Vector3 (transform.position.x, 
                                       transform.position.y + 2.2f, 
                                       transform.position.z), 
                          Quaternion.identity) as GameObject;
    }
}
