using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowArea : MonoBehaviour
{
    public float slowSpeed;
    public float lifetime;

    void Start()
    {
        StartCoroutine(end());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<UnitCore>().SpeedDown(slowSpeed);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        other.GetComponent<UnitCore>().SpeedUp(slowSpeed);
    }

    IEnumerator end()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
