using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float posY = 0.5f;
    Vector3 startPoint, endPoint;
    [SerializeField] Vector3 direction;
    [SerializeField] bool isTouch;
    [SerializeField] bool isChecking;
    [SerializeField] bool isMoving;
    [SerializeField] List<int> pathDir;
    [SerializeField] Vector3 desVec;


    [SerializeField] Map map;

    private void Start()
    {
        desVec = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            isTouch = true;
            isChecking = true; // khi ấn chuột bắt đầu trạng thái checking
            startPoint = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isTouch = false;

        }
        if (isTouch)
        {
            endPoint = Input.mousePosition;
            if (endPoint.Equals(startPoint)) return; // nếu điểm đầu trùng điểm cuối thì mặc định dir = vector zero, nên ko checking valid move
            Vector3 dir = (endPoint - startPoint).normalized; // đã xác định đc hướng
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            angle = (angle + 360) % 360;
            if ((angle >= 315 && angle < 360) || (angle >= 0 && angle < 45))
            {
                direction = Vector3.right;
            }
            else if (angle >= 45 && angle < 135)
            {
                direction = Vector3.forward;
            }
            else if (angle >= 135 && angle < 225)
            {
                direction = Vector3.left;
            }
            else if (angle >= 225 && angle < 315)
            {
                direction = Vector3.back;
            }

            // khi biết hướng bắt đầu checking
            if (isChecking)
            {
                desVec = CheckValidMove(direction); // check 1 lan
            }
        }
        if (desVec.Equals(transform.position)) //neu điểm đến trùng với vị trí hiện tại player thì setMoving = false;
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (isMoving)
        {
            MovingPlayer();
        }
    }

    private Vector3 CheckValidMove(Vector3 dir)
    {
        isChecking = false; // để false luôn chỉ chạy 1 lần duy nhất để check
        pathDir.Clear();
        Vector2 posPlayerInMatrix = new Vector2(transform.position.x, transform.position.z);
        int valueInRow = Mathf.RoundToInt(posPlayerInMatrix.x);
        int valueInColumn = Mathf.RoundToInt(posPlayerInMatrix.y);

        int lastValueRow = 0;
        int lastValueColumn = 0;
        if (dir == Vector3.forward)
        {
            for (int i = 1; i < map.MapMatrix.GetLength(0); i++)
            {
                if (IsValidPos(valueInRow - i, valueInColumn)) //do nguoc chieu nen phai - i
                {
                    int valueMatrix = map.MapMatrix[valueInRow - i, valueInColumn];
                    if (valueMatrix == 0 && pathDir.Count == 0)
                    {
                        return transform.position;
                    }
                    else if (valueMatrix == 0)
                    {
                        break;
                    }
                    lastValueRow = valueInRow - i;
                    lastValueColumn = valueInColumn;
                    pathDir.Add(valueMatrix);
                }
                else if (pathDir.Count == 0)
                {
                    return transform.position;
                }

            }
            return new Vector3(lastValueRow, posY, lastValueColumn);
        }
        else if (dir == Vector3.back)
        {
            for (int i = 1; i < map.MapMatrix.GetLength(0); i++)
            {
                if (IsValidPos(valueInRow + i, valueInColumn))
                {
                    int valueMatrix = map.MapMatrix[valueInRow + i, valueInColumn];
                    if (valueMatrix == 0 && pathDir.Count == 0)
                    {
                        return transform.position;
                    }
                    else if (valueMatrix == 0)
                    {
                        break;
                    }
                    lastValueRow = valueInRow + i;
                    lastValueColumn = valueInColumn;
                    pathDir.Add(valueMatrix);
                }
                else if (pathDir.Count == 0)
                {
                    return transform.position;
                }
            }
            return new Vector3(lastValueRow, posY, lastValueColumn);
        }
        else if (dir == Vector3.left)
        {
            for (int i = 1; i < map.MapMatrix.GetLength(1); i++)
            {
                if (IsValidPos(valueInRow, valueInColumn - i))
                {
                    int valueMatrix = map.MapMatrix[valueInRow, valueInColumn - i];
                    if (valueMatrix == 0 && pathDir.Count == 0)
                    {
                        return transform.position;
                    }
                    else if (valueMatrix == 0)
                    {
                        break;
                    }
                    lastValueRow = valueInRow;
                    lastValueColumn = valueInColumn - i;
                    pathDir.Add(valueMatrix);
                }
                else if (pathDir.Count == 0)
                {
                    return transform.position;
                }
            }
            return new Vector3(lastValueRow, posY, lastValueColumn);
        }
        else if (dir == Vector3.right)
        {
            for (int i = 1; i < map.MapMatrix.GetLength(1); i++)
            {
                if (IsValidPos(valueInRow, valueInColumn + i))
                {
                    int valueMatrix = map.MapMatrix[valueInRow, valueInColumn + i];
                    if (valueMatrix == 0 && pathDir.Count == 0)
                    {
                        return transform.position;
                    }
                    else if (valueMatrix == 0)
                    {
                        break;
                    }
                    lastValueRow = valueInRow;
                    lastValueColumn = valueInColumn + i;
                    pathDir.Add(valueMatrix);
                }
                else if (pathDir.Count == 0)
                {
                    return transform.position;
                }
            }
            return new Vector3(lastValueRow, posY, lastValueColumn);
        }
        return transform.position; //nếu hướng là vector zero thì chả đúng vị trị đang đứng của player
    }

    private void MovingPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, desVec, speed * Time.deltaTime);
    }


    private int ConvertPosPlayerToValueInMatrix()
    {
        Vector2 posPlayerInMatrix = new Vector2(transform.position.x, transform.position.z);
        int valueInRow = Mathf.RoundToInt(posPlayerInMatrix.x);
        int valueInColumn = Mathf.RoundToInt(posPlayerInMatrix.y);

        if (IsValidPos(valueInRow, valueInColumn))
        {
            int valueMatrix = map.MapMatrix[valueInRow, valueInColumn];
            print($"Current value: {valueMatrix}");
            return valueMatrix;
        }
        else
        {
            print("invalid");
            return -1;
        }
    }

    private bool IsValidPos(int i, int j)
    {
        return i >= 0 && i < map.MapMatrix.GetLength(0) && j >= 0 && j < map.MapMatrix.GetLength(1);
    }
}

