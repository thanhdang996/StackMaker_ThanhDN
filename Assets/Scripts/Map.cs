using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] int row;
    [SerializeField] int column;
    int[,] map;
    public int[,] MapMatrix => map;

    Brick[,] brickObjects;
    public Brick[,] BrickObjects => brickObjects;

    [SerializeField] Brick brickPrefab;
    [SerializeField] PlayerMovement player;



    public void GenerateNewMap(int level)
    {
        DestroyAllBrick();
        AddTextToMatrixArray(level);
        SetBrickToMatrixArray();
    }

    private void AddTextToMatrixArray(int level)
    {
        string textMap = Resources.Load<TextAsset>($"Maps/Map{level}").text;
        string[] splitRow = textMap.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        row = splitRow.Length; //25
        column = splitRow[0].Split(",").Length; //13
        map = new int[row, column];
        brickObjects = new Brick[row, column];

        for (int i = 0; i < row; i++)
        {
            var tmp = splitRow[i].Split(",");
            for (int j = 0; j < column; j++)
            {
                var num = int.Parse(tmp[j]);
                map[i, j] = num;
            }
        }
    }

    private void SetBrickToMatrixArray()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < column; j++)
            {
                Vector3 position = new Vector3(i, 0, j);
                Brick brick;
                if (map[i, j] == 0)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
                    brickObjects[i, j] = brick;
                    brick.brickType = Brick.BrickType.Void;
                }
                if (map[i, j] == 1)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
                    brickObjects[i, j] = brick;
                    brick.brickType = Brick.BrickType.CanEat;
                }
                else if (map[i, j] == 2)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
                    brickObjects[i, j] = brick;
                    brick.brickType = Brick.BrickType.MinusBrick;
                }
                else if (map[i, j] == 3)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
                    brickObjects[i, j] = brick;
                    brick.brickType = Brick.BrickType.StartPos;
                    Vector3 posBrick = brick.transform.position;
                    player.transform.position = new Vector3(posBrick.x, 0, posBrick.z);
                }
                else if (map[i, j] == 4)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity, transform);
                    brickObjects[i, j] = brick;
                    brick.brickType = Brick.BrickType.EndPos;
                }
            }
        }
    }

    public void DestroyAllBrick()
    {
        Brick[] brickRemains = transform.GetComponentsInChildren<Brick>();
        foreach (var brickChild in brickRemains)
        {
            Destroy(brickChild.gameObject);
        }
    }
}
