using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCore : MonoBehaviour
{
    public GameCore core; // ядро игры
    public string controlScheme = "wasdf";
    
    private int currentLap = 1; // текущий круг
    private bool direction = true; // true = вправо, false - влево
    private GameObject unit;
    private UnitCore unitcore;
    private Vector2 planPos;
    private float unitSpeed;
    public List<string> activeButtons;

    string up;
    string left;
    string down;
    string right;
    string auto;
    string spell;
    string kill1;
    string kill2;

    void Start()
    {
        activeButtons = new List<string>();
        SetControls(controlScheme);
        //txtForLaps.text = txtForLaps.text.Substring(0, 10) + currentLap.ToString();
    }

    void Update()
    {
        if (unit == null)
            Spawn();

        if (unitcore != null)
        {
            // каждая дорога разделена на слои
            unit.layer = core.PosLayer(unitcore.ground.transform.position.y, direction);
            unit.GetComponent<Renderer>().sortingOrder = unit.layer;
            // выведем кулдаун спелла
            //txtForCD.text = unitcore.CoolDownLeft().ToString();

            if (Input.GetKeyDown(GetKey("right")))
                CallEvent("right.down");
            else
            if (Input.GetKeyDown(GetKey("left")))
                CallEvent("left.down");
            else
            if (Input.GetKeyUp(GetKey("right")))
                CallEvent("right.up");
            else
            if (Input.GetKeyUp(GetKey("left")))
                CallEvent("left.up");

            if (Input.GetKeyDown(GetKey("down")))
                CallEvent("down.down");
            else
            if (Input.GetKeyDown(GetKey("up")))
                CallEvent("up.down");
            else
            if (Input.GetKeyUp(GetKey("down")))
                CallEvent("down.up");
            else
            if (Input.GetKeyUp(GetKey("up")))
                CallEvent("up.up");

            if (Input.GetKeyDown(GetKey("auto")))
                CallEvent("auto.click");
            if (Input.GetKeyDown(GetKey("spell")))
                CallEvent("spell.click");
        }
    }

    public void CallEvent(string input)
    {
        if (unitcore == null)
            return;

        string button = input.Substring(0, input.IndexOf('.'));
        string type = input.Substring(input.IndexOf('.') + 1);

        if (button == "auto")
            unitcore.SetAutoMove(null);
        else
        if (button == "spell" && unitcore.SpellState())
            StartCoroutine(unitcore.useSpell());
        else
        if (type == "down")
            activeButtons.Add(button);
        else
        if (type == "up")
            if (activeButtons.Remove(button) && !unitcore.AutoMoveState())
                unitcore.speed.able = false;
    }

    public GameObject CurrentUnit()
    { return unit; }

    public float RoadBorder(string side)
    { return core.RoadBorder(direction, side); }

    public void ChangeLine(bool changeLap)
    {
        if (changeLap == true)
            currentLap += 1;

        direction = changeLap;
        //txtForLaps.text = txtForLaps.text.Substring(0, 10) + currentLap.ToString();
    }

    public string GetKey(string key)
    {
        switch (key)
        {
            case "up":
                return up;
            case "left":
                return left;
            case "down":
                return down;
            case "right":
                return right;
            case "auto":
                return auto;
            case "spell":
                return spell;
            case "kill1":
                return kill1;
            case "kill2":
                return kill2;
            default:
                return null;
        }
    }

    void SetControls(string scheme)
    {
        if (scheme == "wasdf")
        {
            up = "w";
            left = "a";
            down = "s";
            right = "d";
            auto = "f";
            spell = "q";
            kill1 = "1";
            kill2 = "2";
        }
        else if (scheme == "arrow")
        {
            up = "up";
            left = "left";
            down = "down";
            right = "right";
            auto = "return";
            spell = "right shift";
            kill1 = "[1]";
            kill2 = "[2]";
        }
    }

    void Spawn() // Создание юнита
    {
        int rnd = Random.Range(0, core.allUnits.Length);

        if (direction == true)
        {
            unit = Instantiate (core.allUnits[rnd], 
                                core.respawn1.transform.position, 
                                Quaternion.identity) as GameObject;

            planPos = new Vector2(core.checkpoint1.transform.position.x,
                                  core.checkpoint1.transform.position.y);
        }
        else if (direction == false)
        {
            unit = Instantiate (core.allUnits[rnd],
                                core.respawn2.transform.position, 
                                Quaternion.identity) as GameObject;

            planPos = new Vector2(core.checkpoint2.transform.position.x,
                                  core.checkpoint2.transform.position.y);

            unit.transform.localScale = new Vector3 (unit.transform.localScale.x * -1, 
                                                     unit.transform.localScale.y, 
                                                     unit.transform.localScale.z);
        }

        unitcore = unit.GetComponent<UnitCore>();
        unitcore.SetMoveTarget(planPos);
        unitcore.SetPlayer(this);
    }
}
