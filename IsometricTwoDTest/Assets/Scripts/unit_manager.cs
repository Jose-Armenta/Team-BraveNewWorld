﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class unit_manager : MonoBehaviour
{
    // External Classes//
    import_manager import_manager;  // Import_Manager Class that facilitates cross class, player, and server function calls.

    // Public Global Variables //
    public GameObject asianMelee;     // The GameObject for the asian melee character.
    public GameObject asianRanged;    // The GameObject for the asian range character.
    public GameObject asianTank;      // The GameObject for the asian tank character.
    public GameObject asianChampion;  // The GameObject for the asian champion character.
    public GameObject greekChampion;  // The GameObject for the greek champion character.
    public GameObject greekMelee;     // The GameObject for the greek melee character.
    public GameObject greekRanged;    // The GameObject for the greek range character.
    public GameObject greekTank;      // The GameObject for the greek tank character.
    public GameObject vikingChampion; // The GameObject for the viking champion character.
    public GameObject vikingMelee;    // The GameObject for the viking melee character.
    public GameObject vikingRanged;   // The GameObject for the viking range character.
    public GameObject vikingTank;     // The GameObject for the viking tank character.

    // Private Global Variables //
    private string championName = "champion"; // The name of the champion cur
    private int randomTile = 0;

    // Start is called before the first frame update
    void Start()
    {
        import_manager = GameObject.Find("network_manager").GetComponent<import_manager>(); // Connects to the import_manager.
    }

    // tiles = and array of tile names.
    private void finalize_champion(string[] tiles)
    {
        GameObject tile = GameObject.Find(tiles[(int)(this.randomTile / tiles.Length)]);
        Vector3 tilePosition = tile.transform.position;
        tilePosition.z -= tile.GetComponent<Renderer>().bounds.size.z;

        if (tile.name.Split('_')[0] == "asian")
        {
            asianChampion.name = championName;
            Instantiate(asianChampion, tilePosition, Quaternion.identity);
            //import_manager.run_function("asianChampion", "set_current_tile", new string[1] { tile.name });

        }
        else if (tile.name.Split('_')[0] == "viking")
        {
            vikingChampion.name = championName;
            Instantiate(vikingChampion, tilePosition, Quaternion.identity);
        }
        else if (tile.name.Split('_')[0] == "greek")
        {
            greekChampion.name = championName;
            Instantiate(greekChampion, tilePosition, Quaternion.identity);
        }

        Vector3 cameraPosition = GameObject.Find("Main Camera").transform.position;
        cameraPosition.y = tilePosition.y;
        cameraPosition.x = tilePosition.x;

        GameObject.Find("Main Camera").transform.position = cameraPosition;
    }

    // Parameters = [string civilization, string championName, int randomTile]
    public void add_champion(string[] parameters)
    {
        this.randomTile = int.Parse(parameters[2]);
        championName = parameters[0] + "_" + parameters[1];
        import_manager.run_function("Map", "get_land", new string[3] { parameters[0], "unit_manager", "finalize_champion" });
    }

    // Parameters = [string civilization, string unitType, string unitNumber]
    public void add_unit(string[] parameters)
    {
        //    public string get_current(string[] parameters)
        // for tile coordinates, also figure out scaling for buildings
        GameObject tile = GameObject.Find(parameters[0]);
        Vector3 tilePosition = tile.transform.position;
        tilePosition.y += tile.GetComponent<Renderer>().bounds.size.y;

        if (parameters[0] == "asian")
        {
            if (parameters[1] == "Melee")
            {
                asianMelee.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(asianMelee, tilePosition, Quaternion.identity);
            }
            else if (parameters[1] == "Ranged")
            {
                asianRanged.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(asianRanged, tilePosition, Quaternion.identity);
            }
            else if (parameters[1] == "Tank")
            {
                asianTank.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(asianTank, tilePosition, Quaternion.identity);
            }
        }
        else if (parameters[0] == "greek")
        {
            if (parameters[1] == "Melee")
            {
                greekMelee.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(greekMelee, tilePosition, Quaternion.identity);
            }
            else if (parameters[1] == "Ranged")
            {
                greekRanged.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(greekRanged, tilePosition, Quaternion.identity);
            }
            else if (parameters[1] == "Tank")
            {
                greekTank.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(greekTank, tilePosition, Quaternion.identity);
            }
        }
        else if (parameters[0] == "viking")
        {
            if (parameters[1] == "Melee")
            {
                vikingMelee.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(vikingMelee, tilePosition, Quaternion.identity);
            }
            else if (parameters[1] == "Ranged")
            {
                vikingRanged.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(vikingRanged, tilePosition, Quaternion.identity);
            }
            else if (parameters[1] == "Tank")
            {
                vikingTank.name = parameters[0] + "_" + parameters[1] + "_" + parameters[2];
                Instantiate(vikingTank, tilePosition, Quaternion.identity);
            }
        }
    }

    // Removes the all units on the map.
    // Parameters = []
    public void remove_all_units(string[] parameters)
    {
        object[] sceneGameObjects = GameObject.FindSceneObjectsOfType(typeof(GameObject));
        Debug.Log("Removing the units");
        foreach (GameObject sceneObject in sceneGameObjects)
        {
            if (Regex.IsMatch(sceneObject.name.ToLower(), "asian_*", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(sceneObject.name.ToLower(), "greek_*", RegexOptions.IgnoreCase) ||
                Regex.IsMatch(sceneObject.name.ToLower(), "viking_*", RegexOptions.IgnoreCase))
            {
                Destroy(sceneObject);
            }
        }
    }
}
