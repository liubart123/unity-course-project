﻿using Assets.GamePlay.Scripts.Building;
using Assets.GamePlay.Scripts.Player;
using Assets.GamePlay.Scripts.Waves;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputControl : MonoBehaviour
{
    public MyPlayer owner;
    protected CameraMove cameraMove;
    protected new Camera camera;
    protected ETypeOfInputAction typeOfAction;
    public ETypeOfInputAction TypeOfAction
    {
        get
        {
            return typeOfAction;
        }
        set
        {
            typeOfAction = value;
            textFieldForCurrentOperation.GetComponent<Text>().text = value.ToString();
        }
    }
    protected GuiControl guiControl;
    public GameObject textFieldForCurrentOperation;

    protected Action revertState;
    public void SetTypeOfAction(string t)
    {
        if (t == ETypeOfInputAction.destroy.ToString())
        {
            TypeOfAction = ETypeOfInputAction.destroy;
        }
        else if (t == ETypeOfInputAction.creatingBonusConveyor.ToString())
        {
            TypeOfAction = ETypeOfInputAction.creatingBonusConveyor;
        }
    }
    public enum ETypeOfInputAction
    {
        nothing,
        build,
        destroy,
        showTowerInfo,
        creatingBonusConveyor,
        changingBlockType
    }
    // Start is called before the first frame update
    public virtual void Start()
    {
        cameraMove = GetComponent<CameraMove>();
        camera = FindObjectOfType<Camera>();
        //guiControl = FindObjectOfType<GuiControl>();
    }
    public virtual void Initialize(MyPlayer pl)
    {
        owner = pl;
        guiControl = pl.guiControl;
        WaveManager.OnWaveStart += OperationsCancelation;
    }
    private void OnDestroy()
    {
        WaveManager.OnWaveStart -= OperationsCancelation;

    }
    // Update is called once per frame
    public virtual void Update()
    {
        //wasd
        if (Input.GetAxis("Horizontal")!=0 || Input.GetAxis("Vertical")!=0){
            cameraMove.MoveCamera(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        }
        //scrolling
        if (Input.mouseScrollDelta.y != 0){
            cameraMove.ScrollCamera(-(int)Mathf.Sign(Input.mouseScrollDelta.y));
        }
        //EventSystem es = FindObjectOfType<EventSystem>();
        //if (Input.GetMouseButtonDown(0))
        //{
        //    //was this click on ui or gane object
        //    if (EventSystem.current.IsPointerOverGameObject())
        //    {

        //    } else
        //    {
        //        GameObject blockObj = BlocksGenerator.GetGameObjectBlock(camera.ScreenToWorldPoint(Input.mousePosition));
        //        if (blockObj != null)
        //        {
        //            Block currentBlock = blockObj.GetComponent<Block>();
        //            if (currentBlock != null)
        //            {
        //                if (typeOfAction == ETypeOfInputAction.build)
        //                {
        //                    owner.builder.BuildBuildingOnBlock(currentBlock);
        //                } else if (typeOfAction == ETypeOfInputAction.destroy) {
        //                    currentBlock.GetBuilding()?.GetComponent<Building>()?.Die();
        //                }
        //            }
        //        }
        //    }
        //}
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OperationsCancelation();
        }
    }
    public void OperationsCancelation()
    {
        if (TypeOfAction == ETypeOfInputAction.nothing)
        {
            if (!guiControl.menuPanel.activeSelf)
                guiControl.OpenMenuPanel();
            else
                guiControl.CloseAllPanels();
        }
        else
        {
            TypeOfAction = ETypeOfInputAction.nothing;
        }
    }
}
