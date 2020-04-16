﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksGenerator : MonoBehaviour
{
    public static int width = 60;
    public static int height = 40;
    public GameObject blockObject;
    public static GameObject[,] blockArray;
    public Vector2 startPos;
    public Vector2 BlockSize;
    // Start is called before the first frame update
    void Start()
    {
        GenerateBlocks();
    }

    // Update is called once per frame
    void Update()
    {

    }
    protected void GenerateBlocks()
    {
        blockArray = new GameObject[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Vector2 pos = startPos;
                pos.x += i * BlockSize.x;
                pos.y += j * BlockSize.y;
                blockArray[i, j] = Instantiate(blockObject, pos, transform.rotation);
                blockArray[i, j].transform.SetParent(transform);
                blockArray[i, j].GetComponent<Block>().indexes = new Vector2Int(i, j);
            }
        }
    }
    public static Block GetBlock(Vector2Int indexes)
    {
        return GetBlock(indexes.x, indexes.y);
    }
    public static Block GetBlock(int x, int y)
    {
        return GetGameObject(x, y)?.GetComponent<Block>();
    }
    public static GameObject GetGameObject(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
            return null;
        return blockArray[x, y];
    }
    public static GameObject GetGameObjectBlock(Vector2 pos)
    {
        int layerMask = 1 << 10;
        RaycastHit2D hit = Physics2D.Raycast(pos,pos,Mathf.Infinity,layerMask);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    public static Block GetBlock(Vector2 pos)
    {
        return GetGameObjectBlock(pos)?.GetComponent<Block>();
    }
}