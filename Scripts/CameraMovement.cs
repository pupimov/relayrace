using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private Vector2 startPos;
    private Vector3 targetPos;
    private Camera cam;

    public PlayerCore player;
    public float speed = 5f;
    public Vector2 xEdges;      
    public Vector2 yEdges;          

    void Start()
    {
        cam = GetComponent<Camera>();
        targetPos = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Lerp(transform.position.x, targetPos.x, speed * Time.deltaTime),
            Mathf.Lerp(transform.position.y, targetPos.y, speed * Time.deltaTime),
            transform.position.z);
    }

    public void OnClick(Vector3 mousePos)
    {
        startPos = cam.ScreenToWorldPoint(mousePos); 
    }

    public void SetTargetPos(Vector3 mousePos)
    {
        targetPos = new Vector3(
            Mathf.Clamp(
                transform.position.x - cam.ScreenToWorldPoint(mousePos).x + startPos.x, 
                xEdges.x + Camera.main.orthographicSize * Camera.main.aspect, 
                xEdges.y - Camera.main.orthographicSize * Camera.main.aspect),
            Mathf.Clamp(
                transform.position.y - cam.ScreenToWorldPoint(mousePos).y + startPos.y, 
                yEdges.x + Camera.main.orthographicSize, 
                yEdges.y - Camera.main.orthographicSize),
            transform.position.z);
    }

    //void CameraMove()
    //{
    //    // -27;8    27;8
    //    // -27;-8   27;-8

    //    if (Input.mousePosition.x <= 10) // влево
    //        camPos.x -= Time.deltaTime * camSpeed;

    //    if (Input.mousePosition.x >= screenWidth - 10) // вправо
    //        camPos.x += Time.deltaTime * camSpeed;

    //    if (Input.mousePosition.y <= 10) // вниз
    //        camPos.y -= Time.deltaTime * camSpeed;

    //    if (Input.mousePosition.y >= screenHeight - 10) // вверх
    //        camPos.y += Time.deltaTime * camSpeed;

    //    transform.position = new Vector3(Mathf.Clamp(camPos.x, 
    //                                                 xEdges.x + Camera.main.orthographicSize, 
    //                                                 xEdges.y - Camera.main.orthographicSize),
    //                                     Mathf.Clamp(camPos.y, 
    //                                                 yEdges.x + Camera.main.orthographicSize * Camera.main.aspect, 
    //                                                 yEdges.y - Camera.main.orthographicSize * Camera.main.aspect),
    //                                     camPos.z);
    //}
}
