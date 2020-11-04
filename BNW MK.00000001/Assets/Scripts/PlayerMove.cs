﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : TileMove
{
    Vector3 targetPosition;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward);


        if (!moving)
        {
            FindSelectableTiles();
            // Registers clicks  
            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "Tile")
                    {
                        Tile t = hit.collider.GetComponent<Tile>();
                        // if tile clicked 
                        if (t.selectable)
                        {
                            moving = true;
                            targetPosition = t.transform.position;
                            Move(targetPosition);
                        }
                    }
                }
            }
        }
    }

    void Move(Vector3 location)
    {
        this.transform.LookAt(targetPosition);

        this.transform.position = new Vector3(targetPosition.x, this.transform.position.y, targetPosition.z);

        this.transform.rotation = Quaternion.identity;

     
        moving = false;
        //TurnManager.EndTurn();
       

    }
}