using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBird : UnitCore
{
    public GameObject beak;
    public GameObject bulletPrefab;
    public int bulletDamage;
    private GameObject bullet;

    protected override void OnSpell()
    {
        if (Direction() == "right")
            bullet = Instantiate(bulletPrefab,
                     new Vector3(beak.transform.position.x + 1,
                                 beak.transform.position.y,
                                 beak.transform.position.z),
                     Quaternion.identity) as GameObject;

        else if (Direction() == "left")
            bullet = Instantiate(bulletPrefab,
                     new Vector3(beak.transform.position.x - 1,
                                 beak.transform.position.y,
                                 beak.transform.position.z),
                     Quaternion.identity) as GameObject;

        if (bullet != null)
        {
            bullet.GetComponent<Damager>().SetDirection(Direction());
            bullet.GetComponent<Damager>().SetDamage(bulletDamage);
            bullet.layer = gameObject.layer;
            bullet.GetComponent<Renderer>().sortingOrder = gameObject.GetComponent<Renderer>().sortingOrder;
        }
    }
}