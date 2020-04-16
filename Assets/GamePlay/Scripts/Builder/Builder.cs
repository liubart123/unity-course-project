﻿using Assets.GamePlay.Scripts.Building;
using Assets.GamePlay.Scripts.Player;
using Assets.GamePlay.Scripts.Tower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
    public Player owner;
    [SerializeField]
    protected Building currentBuilding;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBuilding(Tower tower)
    {
        currentBuilding = tower;
        owner.inputControl.IsBuilding = true;
    }
    public void BuildTower(Block block)
    {
        if (currentBuilding != null && !block.HasBuilding())
        {
            GameObject res = Instantiate(currentBuilding.gameObject, block.transform.position, block.transform.rotation);
            res.transform.parent = block.transform;
            res.GetComponent<Building>().owner = owner;
            res.GetComponent<Building>().Initialize();
        }
    }
}