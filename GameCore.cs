using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct RoadLayer
{
    public int layer;
    public float yMax;
    public float yMin;
}

public class GameCore : MonoBehaviour
{
    public PlayerCore tmpPlayer;
    public int playersCount; // кол-во игроков
    public GameObject player; // объект игрока
    public GameObject[] allUnits; // все игровые персонажи
    public GameObject respawn1;
    public GameObject respawn2;
    public GameObject checkpoint1;
    public GameObject checkpoint2;
    public GameObject road1;
    public GameObject road2;

    private GameObject temp;
    private PlayerCore[] allPlayers; // все игроки
    private List<RoadLayer> road1layers;
    private List<RoadLayer> road2layers;

    void Start()
    {
        // посчитаем границы слоев для обеих дорог
        road1layers = SetLayers(road1, true);
        road2layers = SetLayers(road2, false);

        // создадим игроков
        allPlayers = new PlayerCore[playersCount];
        allPlayers[0] = tmpPlayer;

        for (int i = 1; i < playersCount; i++)
        {
            temp = Instantiate(player) as GameObject;
            allPlayers[i] = temp.GetComponent<PlayerCore>();
            allPlayers[i].core = this;
            allPlayers[i].controlScheme = "wasdf";
            temp = null;
        }
    }

    private List<RoadLayer> SetLayers(GameObject road, bool isTop)
    {
        float layerHeight;
        Bounds bounds;
        List<RoadLayer> roadLayers = new List<RoadLayer>();

        bounds = road.GetComponent<Collider2D>().bounds;
        layerHeight = Mathf.Abs(RoadBorder(isTop, "up") - RoadBorder(isTop, "down")) / 9f;

        for (int i = 1; i <= 9; i++) // разобьем дорогу на слои от верхнего к нижнему
        {
            roadLayers.Add(new RoadLayer
            {
                layer = i + 10, 
                yMax = bounds.max.y - layerHeight * (i - 1),
                yMin = bounds.max.y - layerHeight * i
            });
        }

        return roadLayers;
    }

    public int PosLayer(float yPos, bool isTop)
    {
        List<RoadLayer> roadLayers;

        if (isTop == true)
            roadLayers = road1layers;
        else
            roadLayers = road2layers;

        for (int i = 0; i < 9; i++)
        {
            if (roadLayers[i].yMin <= yPos && yPos <= roadLayers[i].yMax)
                return roadLayers[i].layer;
        }

        return 0;
    }

    public float RoadBorder(bool isTop, string side)
    {
        Bounds current;
        if (isTop == true) // верхняя дорога
            current = road1.GetComponent<Collider2D>().bounds;
        else              // нижняя дорога
            current = road2.GetComponent<Collider2D>().bounds;

        switch (side)
        {
            case "up":
                return (current.max.y);
            case "down":
                return (current.min.y);
            case "right":
                return (current.max.x);
            case "left":
                return (current.min.x);
            default:
                return 0f;
        }
    }

    public Vector2 GetPlayerPos(int id)
    {
        return new Vector2(allPlayers[id-1].CurrentUnit().transform.position.x,
                           allPlayers[id-1].CurrentUnit().transform.position.y);
    }
}
