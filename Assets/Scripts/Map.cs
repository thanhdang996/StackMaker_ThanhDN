using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public int row;
    public int column;
    public int[,] map;

    [SerializeField] public Brick brickPrefab;
    [SerializeField] public PlayerMovement player;

    private void Start()
    {
        Application.targetFrameRate = 30;

        AddTextToMatrixArray();
        SetBrickToMatrixArray();
    }

    private void AddTextToMatrixArray()
    {
        string textMap = Resources.Load<TextAsset>("Maps/Map1").text;
        string[] splitRow = textMap.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        row = splitRow.Length;
        column = splitRow[0].Split(",").Length;
        map = new int[row, column];

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
        for (int y = 0; y < row; y++)
        {
            for (int x = 0; x < column; x++)
            {
                Vector3 position = new Vector3(x, 0, -y);
                Brick brick;
                if (map[y, x] == 0)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity);
                    brick.brickType = Brick.BrickType.Void;
                }
                else if (map[y, x] == 1)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity);
                    brick.brickType = Brick.BrickType.CanEat;
                }
                else if (map[y, x] == 2)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity);
                    brick.brickType = Brick.BrickType.MinusBrick;
                }
                else if (map[y, x] == 3)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity);
                    brick.brickType = Brick.BrickType.StartPos;
                    Vector3 posBrick = brick.transform.position;
                    player.transform.position = new Vector3(posBrick.x, 0.5f, posBrick.z);
                }
                else if (map[y, x] == 4)
                {
                    brick = Instantiate(brickPrefab, position, Quaternion.identity);
                    brick.brickType = Brick.BrickType.EndPos;
                }
            }
        }
    }

}
