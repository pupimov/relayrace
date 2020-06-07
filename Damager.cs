using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    public float speed;
    public float range;

    private int damage;
    private string direction;
    private float deathPos;

    public void SetDamage(int inDamage)
    { damage = inDamage; }

    public void SetDirection(string indirection)
    { direction = indirection; }

    void Start()
    {
        deathPos = transform.position.x + range;
    }

    void Update()
    {
        if (deathPos <= transform.position.x)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        if (direction == "right")
        {
            transform.Translate(new Vector2(1f, 0f) * speed * Time.deltaTime);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),
                                                         transform.localScale.y,
                                                         transform.localScale.z);
        }
        else if (direction == "left")
        {
            transform.Translate(new Vector2(-1f, 0f) * speed * Time.deltaTime);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1,
                                                         transform.localScale.y,
                                                         transform.localScale.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        UnitCore unit = other.GetComponent<UnitCore>();
        if (unit != null)
        {
            int hp = unit.DealDamage(damage);
            Destroy(gameObject);
        }
    }
}
