﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;
using UnityEngine.UIElements;


public class Tile : MonoBehaviour
{

    // External Classes//
    import_manager import_manager;  // Import_Manager Class that facilitates cross class, player, and server function calls.
    map_manager map_manager;

    public bool walkable = true;
    public bool current = false; // if the player is currently using this tile
    public bool occupied = false; // if there is a character currently on this tile
    public bool target = false;
    public bool selectable = false;

    public GameObject currentchar = null; // the character currently on this tile. Or about to be moved to this tile.
    public List<Tile> adjacencyList = new List<Tile>();

    //Needed BFS (breadth first search)
    public bool visited = false;
    public Tile parent = null;
    public int distance = 0;



    // Private Variables //
    private Color  realColor;    // The color the tile should be without any highlights.
    private string civilization; // The number associated with the civ that owns this land. -1 = water, 0 = asian, 1 = greek, 2 = viking
    private int[]  grid;         // Stores the position of the Tile in the virtual grid. [x position, y position]

    // Use this for initialization
    void Start () 
	{
        import_manager = GameObject.Find("network_manager").GetComponent<import_manager>(); // Connects to the import_manager.

        map_manager = GameObject.Find("Map").GetComponent<map_manager>();

        realColor = this.GetComponent<Renderer>().material.color;

    }

    // Update is whenever needed
    public void Updateme()
    {
        // determine if there is a character currently on the tile.
        Debug.Log("This is currentchar = " + currentchar + " occupied = " + occupied + " current = " + current);
        if (currentchar != null)
        {
            occupied = true;
        }
        else
        {
            occupied = false;
        }

        if (current && occupied)
        {
            this.GetComponent<Renderer>().material.color = Color.magenta;
        }
        else if (current)
        {
            this.GetComponent<Renderer>().material.color = Color.cyan;
        }
        else if (occupied)
        {
            this.GetComponent<Renderer>().material.color = Color.red;
             //this.GetComponent<Renderer>().material.color = realColor;
        }
        else if (target)
        {
            this.GetComponent<Renderer>().material.color = Color.white;
        }
        else if (selectable)
        {
            this.GetComponent<Renderer>().material.color = Color.blue;
             //this.GetComponent<Renderer>().material.color = realColor;
        }
        else
        {
            this.GetComponent<Renderer>().material.color = realColor;
        }
    }

    // Set current to this tile when it gets clicked
    public void OnMouseDown()
    {
        // If the tile is selectable and open, then move the current character to this tile
        if (selectable && (occupied == false))
        {
            import_manager.run_function("Map", "get_current_char", new string[2] { grid[0].ToString(), grid[1].ToString() });
            import_manager.run_function_all(currentchar.name, "switch_selectable_tile", new string[0] { });
            import_manager.run_function_all(currentchar.name, "move", new string[2] {grid[0].ToString(), grid[1].ToString()});
        }
        else if(occupied) // and in range, and not a friendly civ
        {
            // check if this characters civ is the same as the character clicking on it
            
        }
         
        import_manager.run_function("Map", "unselect_tile", new string[0] { });
        import_manager.run_function("Map", "set_current", new string[2] { grid[0].ToString(), grid[1].ToString()});
        current = true;

        Updateme();
        PlayerMove temp = currentchar.GetComponent<PlayerMove>(); 
        temp.set_selectable(); // this allows you to move right after youve moved, this will be disabled when we set cooldowns.
    }

    public void CheckTile()
    {
        Updateme();
        int range;

        if (currentchar != null)
        {
            range = currentchar.GetComponent<PlayerMove>().moveRange;
        }
        else
        {
            range = 0;
        }
        List<map_manager.map_item> inrange = new List<map_manager.map_item>();

        for(distance = 1; distance > range; distance++)
        {
            inrange.Add(map_manager.map[grid[0]           , grid[1] + distance]);
            inrange.Add(map_manager.map[grid[0] + distance, grid[1]           ]);
            inrange.Add(map_manager.map[grid[0]           , grid[1] - distance]);
            inrange.Add(map_manager.map[grid[0] - distance, grid[1]           ]);

            inrange.Add(map_manager.map[grid[0] + distance, grid[1] + distance]);
            inrange.Add(map_manager.map[grid[0] + distance, grid[1] - distance]);
            inrange.Add(map_manager.map[grid[0] - distance, grid[1] + distance]);
            inrange.Add(map_manager.map[grid[0] - distance, grid[1] - distance]);
        }

        foreach(map_manager.map_item item in inrange)
        {
            adjacencyList.Add(item.ground.GetComponent<Tile>());
            item.ground.GetComponent<Tile>().selectable = false;
            item.ground.GetComponent<Tile>().Updateme();
        }
    }


    // Set and get functions

    // Unselect this tile
    // parameter = empty array (not used)
    public void unselect(string[] parameter)
    {
        current = false;
        Updateme();

        // if the tile being unselected is occupied, also unselect all nearby tiles
        if (occupied)
        {
            foreach (Tile tile in adjacencyList)
            {
                tile.selectable = false;
                tile.Updateme();
                
                    foreach (Tile tile2 in tile.adjacencyList)
                    {
                        tile2.selectable = false;
                        tile2.Updateme();
                    }
            }
        }
    }

    // Get and Set Functions //

    public void set_civilization(string civ)
    {
        civilization = civ;
    }

    public string get_civilization()
    {
        return civilization;
    }

    public void set_grid(int xPosition, int yPosition)
    {
        grid = new int[2] { xPosition, yPosition };
    }

    public int[] get_grid()
    {
        return grid;
    }

    public bool is_current()
    {
        return current;
    }

    public void set_occupied(string[] parameter)
    {
        occupied = true;
        Debug.Log("Setting Occupied");
        Updateme();
    }
    public void set_unoccupied(string[] parameter)
    {
        occupied = false;
        Updateme();
    }
    public void set_selectable()
    {
        selectable = true;
        Updateme();
    }

    public void set_current_char(string[] newcurrentChar)
    {
        Debug.Log("set_current_char = " + newcurrentChar[0]);

        if (newcurrentChar[0] != "")
        {
            currentchar = GameObject.Find(newcurrentChar[0]);
        }
        
        Updateme();
    }
    public void Reset()
    {
        adjacencyList.Clear();

        current = false;
        target = false;
        selectable = false;

        //visited = false;
        parent = null;
        distance = 0;

        Updateme();
    }
}
