//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public abstract class PrefabCore : MonoBehaviour
//{
//    // урон
//    public bool vulnerable;
//    public int maxHitpoints;
//    private int hitPoints; // текущий уровень здоровья
//    // передвижение
//    public bool moveable;
//    public float maxSpeed;
//    private float speed; // текущая скорость
//    private bool runActive = false; // флаг активности режима автобега
//    private Vector2 planPos; // планируемая позиция
//    private bool moveState;
//    // анимация
//    public bool animated;
//    public float deathPing;
//    protected Animator anim;
//    // видимость
//    public float sightRange;

//    // геттеры и сеттеры
//    public void MoveLock()
//    { moveable = false; }

//    public void MoveUnlock()
//    {
//        if (moveState == true)
//            moveable = true;
//    }

//    public float GetSpeed()
//    { return speed; }

//    public void SetPos(Vector2 pos)
//    { planPos = pos; }

//    public bool GetAutoMove()
//    { return runActive; }

//    public void SetAutoMove(bool active)
//    { 
//        runActive = active;
//        if (anim != null)
//            anim.SetBool("move", runActive);
//    }

//    public void ChangeAutoMove()
//    {
//        runActive = !runActive;
//        if (anim != null)
//            anim.SetBool("move", runActive);
//    }

//    public void ResetSpeed()
//    { speed = maxSpeed; }

//    public void Slow(float slowSpeed)
//    {
//        if (speed > slowSpeed)
//            speed = slowSpeed;
//    }

//    public string GetDirection()
//    {
//        if (transform.localScale.x >= 0)
//            return "right";
//        else
//            return "left";
//    }

//    public void DealDamage(int damage)
//    {
//        IEnumerator death()
//        {
//            hitPoints = 0;
//            moveable = false; // не даем юниту двигаться

//            if (anim != null)
//                anim.SetTrigger("death");

//            yield return new WaitForSeconds(deathPing);
//            Destroy(gameObject);
//        }

//        if (vulnerable == true)
//        {
//            if (hitPoints <= damage)
//                StartCoroutine(death());
//            else
//                hitPoints -= damage;
//        }
//    }

//    public void ChangeDirection(string direction)
//    {
//        if (direction == "right")
//            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),
//                                               transform.localScale.y,
//                                               transform.localScale.z);
//        else if (direction == "left")
//            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1,
//                                               transform.localScale.y,
//                                               transform.localScale.z);
//    }

//    // старт
//    protected virtual void Start()
//    {
//        if (animated == true)
//            anim = GetComponent<Animator>();
//        else
//            anim = null;

//        if (vulnerable == true)
//            hitPoints = maxHitpoints;

//        if (moveable == true)
//        {
//            moveState = moveable;
//            ResetSpeed();
//        }
//    }

//    // передвижение
//    protected virtual void FixedUpdate()
//    {
//        if (runActive == true && planPos != null && moveable == true)
//            AutoMove();
//    }

//    protected void AutoMove()
//    {
//        Vector3 nextPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);

//        if (planPos.x < transform.position.x) // влево
//        {
//            nextPos.x = transform.position.x - speed * Time.deltaTime;
//            ChangeDirection("left");
//        }
//        else if (planPos.x > transform.position.x) // вправо
//        {
//            nextPos.x = transform.position.x + speed * Time.deltaTime;
//            ChangeDirection("right");
//        }

//        if (planPos.y < transform.position.y - 1) // вниз
//            nextPos.y = transform.position.y - speed * Time.deltaTime;
//        else if (planPos.y > transform.position.y + 1) // вверх
//            nextPos.y = transform.position.y + speed * Time.deltaTime;

//        if (transform.position == nextPos)
//        {
//            runActive = false;
//            if (anim != null)
//                anim.SetBool("move", false);
//        }
//        else
//            transform.position = nextPos;
//    }
//}
