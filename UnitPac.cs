using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPac : UnitCore
{
    public float animFps;
    public float stunDuration;
    public int stunDamage;

    CapsuleCollider2D spellCollider;
    Vector2 offsetTemp;
    UnitCore hitUnit;

    protected override void Start()
    {
        base.Start();
        spellCollider = GetComponent<CapsuleCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player" && other.otherCollider == spellCollider)
            hitUnit = other.gameObject.GetComponent<UnitCore>();
    }

    public override IEnumerator useSpell()
    {
        SpellLock(true);
        Animate("spell");

        for (float i = 0; i < animFps; i++)
        {
            if (hitUnit == null)
                yield return new WaitForSeconds(1f / animFps);
            else
                break; // попали
        }

        Animate("stopspell");

        if (hitUnit == null)
            SpellLock(false);
        else
        {
            OnSpell();
            StartCoroutine(coolDown());
        }
    }

    protected override void OnSpell()
    {
        int hp = hitUnit.DealDamage(stunDamage);
        if (hp > 0)
            hitUnit.Stun(stunDuration);
        hitUnit = null;
    }
}
















