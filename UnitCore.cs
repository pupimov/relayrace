using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitCore : MonoBehaviour
{
    protected virtual void Start()
    {
        speed = new Speed(maxSpeed);
        moveState = moveable;
        spellState = spellable;

        anim = animated ? GetComponent<Animator>() : null;
        hitpoints = vulnerable ? maxHitpoints : 0;
    }
    protected virtual void Update()
    {
        if (player != null)
            speed.able = AutoMoveState() ? moveState : player.activeButtons.Count > 0;
    }
    protected virtual void FixedUpdate()
    {
        if (moveState)
        {
            ManualMove();
            if (AutoMoveState())
                AutoMove();

            Animate("move", speed.able);
        }
    }

    // передвижение ------------------------------------------------------ //
    public bool moveable;
    protected bool moveState;
    public float maxSpeed;
    public Speed speed; // текущая скорость
    protected bool runActive = false; // флаг активности режима автобега
    protected Vector2 planPos; // планируемая позиция
    public GameObject ground;

    // передвижение
    protected void ManualMove()
    {
        if (CheckKey("right"))
        {
            Move("right");
            SetAutoMove(false); // сбрасываем автомув
        }
        else
        if (CheckKey("left"))
        {
            Move("left");
            SetAutoMove(false); // сбрасываем автомув
        }

        if (CheckKey("up"))
            Move("up");
        else
        if (CheckKey("down"))
            Move("down");
    }

    protected void AutoMove()
    {
        if ((planPos.x < ground.transform.position.x) && !CheckBorder("left"))
            Move("left");
        else
        if ((planPos.x > ground.transform.position.x) && !CheckBorder("right"))
            Move("right");
    }

    public bool CheckKey(string key)
    { return player.activeButtons.Contains(key) && !CheckBorder(key); }

    public void Move(string direction)
    { 
        Vector2 nextPos = Vector2.zero;

        switch (direction)
        {
            case "right":
                nextPos = Vector2.right;
                SetDirection(direction);
                break;
            case "left":
                nextPos = Vector2.left;
                SetDirection(direction);
                break;
            case "up":
                nextPos = Vector2.up * 0.8f;
                break;
            case "down":
                nextPos = Vector2.down * 0.8f;
                break;
            default:
                break;
        }

        if (nextPos != Vector2.zero)
            transform.Translate(nextPos * Time.deltaTime * speed);
    }

    public bool AutoMoveState()
    { return runActive; }

    public void SetAutoMove(bool? active = null)
    { runActive = active is null ? !runActive : (active ?? false); }

    public void SetMoveTarget(Vector2 pos)
    { planPos = pos; }

    // скорость
    public class Speed
    {
        public bool able;
        float currSpeed;
        float maxSpeed;
        List<float> modifiers;

        public Speed(float inSpeed)
        {
            able = inSpeed > 0f;
            currSpeed = maxSpeed = inSpeed;
            modifiers = new List<float>();
            modifiers.Add(0f);
        }

        public static bool operator ==(Speed o1, float o2)
        { if (o1.get() == o2) return true; else return false; }

        public static bool operator !=(Speed o1, float o2)
        { if (o1.get() != o2) return true; else return false; }

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            Speed tmp = obj as Speed;
            if (tmp as Speed == null)
                return false;

            return tmp.get() == this.get();
        }

        public override int GetHashCode()
        { return (int)currSpeed; }

        public static float operator *(float o1, Speed o2)
        { return o1 * o2.get(); }

        public static Vector2 operator *(Vector2 o1, Speed o2)
        { return o1 * o2.get(); } //new Vector2(o1.x * o2.get(), o2.y * o2.get()); }

        public float get()
        { return able ? ((currSpeed >= 0f) ? currSpeed : 0f) : 0f; }

        public void Down(float downSpeed)
        {
            modifiers.Add(downSpeed - maxSpeed);

            if (currSpeed > downSpeed)
                currSpeed = downSpeed;
        }

        public void ReDown(float downSpeed)
        {
            if (!modifiers.Remove(-downSpeed))
                return;

            float least = 0f;

            foreach (float tmp in modifiers)
                if (tmp < least)
                    least = tmp;

            currSpeed = maxSpeed + least;
        }
    }

    public void SpeedDown(float inSpeed)
    { speed.Down(inSpeed); }

    public void SpeedUp(float inSpeed)
    { speed.ReDown(inSpeed); }

    // блокировка передвижения за границы
    public bool CheckBorder(string side)
    {
        switch (side)
        {
            case "up":
                return ground.transform.position.y > player.RoadBorder(side) - 0.2f;
            case "down":
                return ground.transform.position.y < player.RoadBorder(side) + 0.2f;
            case "right":
                return ground.transform.position.x > player.RoadBorder(side) - 2f;
            case "left":
                return ground.transform.position.x < player.RoadBorder(side) + 2f;
            default:
                return false;
        }
    }

    public void MoveLock(bool inState) 
    {
        if (inState == true)
            moveState = false;
        else if (moveable == true)
            moveState = true;
    }

    // направление взгляда
    public string Direction() 
    {
        if (transform.localScale.x >= 0)
            return "right";
        else
            return "left";
    }

    public void SetDirection(string direction)
    {
        if (direction == "right")
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),
                                               transform.localScale.y,
                                               transform.localScale.z);
        else if (direction == "left")
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1,
                                               transform.localScale.y,
                                               transform.localScale.z);
    }

// здоровье ------------------------------------------------------ //
    public bool vulnerable;
    public int maxHitpoints;
    protected int hitpoints = 0; // текущий уровень здоровья

    public bool AliveState()
    { return hitpoints > 0; }

    public int DealDamage(int damage)
    {
        if (!vulnerable)
            return hitpoints;

        if (hitpoints > damage)
            hitpoints -= damage;
        else
        {
            hitpoints = 0;
            Stun(deathPing, "death");
        }
        return hitpoints;
    }

    public void Stun(float duration, string animType = "stun")
    {
        IEnumerator w8Duration()
        {
            Animate(animType, true);
            MoveLock(true);
            SpellLock(true);
            
            yield return new WaitForSeconds(duration);

            if (animType == "death")
                Destroy(gameObject);
            else
                Animate(animType, false);

            MoveLock(false);
            SpellLock(false);
        }
        StartCoroutine(w8Duration());
    }

// способность ------------------------------------------------------ //
    public bool spellable;
    protected bool spellState;
    public float spellCooldown;
    protected float spellLeftCD = 0f;

    public float CoolDownLeft() // остаток кулдауна
    { return spellLeftCD; }

    public bool SpellState()
    { return spellState; }

    public void SpellLock(bool inState) // не даем кастовать
    {
        if (inState == true)
            spellState = false;
        else if (spellable == true && spellLeftCD == 0f)
            spellState = true;
    }

    protected virtual void OnSpell()
    { /* способность переопределяется */ }

    public virtual IEnumerator useSpell()
    {
        SpellLock(true);
        Animate("spell");
        yield return new WaitForSeconds(spellPing);
        OnSpell();
        StartCoroutine(coolDown());
    }

    protected IEnumerator coolDown()
    {
        SpellLock(true);
        for (float i = spellCooldown; i > 0f; i = i - 1f)
        {
            spellLeftCD = i;
            yield return new WaitForSeconds(1f);
        }

        spellLeftCD = 0f;
        SpellLock(false);
    }

// анимация ------------------------------------------------------ //
    public bool animated;
    public float deathPing;
    public float spellPing;
    protected Animator anim;

    protected virtual void Animate(string name, bool state = false)
    {
        if (anim == null)
            return;

        if (name == "stun" || name == "move")
            anim.SetBool(name, state);
        else
            anim.SetTrigger(name);
    }

    // связь с игроком ------------------------------------------------------ //
    protected PlayerCore player; // объект класса игрока
    protected GameObject checkpoint; // чекпоинт к которому двигается юнит

    public PlayerCore Player()
    { return player; } // без player юнит не может управляться

    public void SetPlayer(PlayerCore inPlayer)
    { player = inPlayer; }
}
