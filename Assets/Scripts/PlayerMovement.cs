using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    Vector3 startPoint, endPoint;
    [SerializeField] Vector3 direction;
    [SerializeField] bool isTouch;
    [SerializeField] bool isMoving;
    [SerializeField] Vector3 desVec;


    [SerializeField] Brick currentBrick;
    [SerializeField] int ownBrick;
    [SerializeField] Stack<Brick> listOwnBrick = new Stack<Brick>();
    [SerializeField] float offsetBrick = 0.2f;



    [SerializeField] Map map;

    private void Start()
    {
        desVec = transform.position;
    }

    private void OnInit()
    {
        if (GetComponent<Rigidbody>() != null)
        {
            Destroy(GetComponent<Rigidbody>());
        }
        enabled = true;
        desVec = transform.position;
        direction = Vector3.zero;
        isMoving = false;
        currentBrick = null;
        ownBrick = 0;
        listOwnBrick.Clear();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isMoving)
        {
            isTouch = true;
            startPoint = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isTouch = false;
        }
        if (isTouch)
        {
            HandleDirection();
        }

        desVec = CheckValidMove(direction); // check liên tục

        if (desVec.Equals(transform.position) || direction == Vector3.zero) //neu điểm đến trùng với vị trí hiện tại player thì setMoving = false;
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (isMoving)
        {
            Brick brick = GetBrickInPlayerCurrentPos(); // brick này đã Instantiate ở map, chỉ lấy vị trị hiện tại rồi gán vào biến brick
            if (currentBrick != brick)
            {
                currentBrick = brick;
                HandleBrickAndAdjustDesVecY(currentBrick);
            }
            MovingPlayer();
        }

    }

    private void HandleDirection()
    {
        if (isMoving) { return; }
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
    }

    private Vector3 CheckValidMove(Vector3 dir)
    {
        Vector2 posPlayerInMatrix = new Vector2(transform.position.x, transform.position.z);
        int valueInRow = Mathf.RoundToInt(posPlayerInMatrix.x);
        int valueInColumn = Mathf.RoundToInt(posPlayerInMatrix.y);

        if (dir == Vector3.forward)
        {
            if (IsValidPos(valueInRow - 1, valueInColumn))
            {
                int valueMatrix = map.MapMatrix[valueInRow - 1, valueInColumn];
                if (valueMatrix != 0)
                {
                    return new Vector3(valueInRow - 1, desVec.y, valueInColumn);
                }
            }
            return new Vector3(valueInRow, desVec.y, valueInColumn);
        }
        else if (dir == Vector3.back)
        {
            if (IsValidPos(valueInRow + 1, valueInColumn))
            {
                int valueMatrix = map.MapMatrix[valueInRow + 1, valueInColumn];
                if (valueMatrix != 0)
                {
                    return new Vector3(valueInRow + 1, desVec.y, valueInColumn);
                }
            }
            return new Vector3(valueInRow, desVec.y, valueInColumn);
        }
        else if (dir == Vector3.left)
        {
            if (IsValidPos(valueInRow, valueInColumn - 1))
            {
                int valueMatrix = map.MapMatrix[valueInRow, valueInColumn - 1];
                if (valueMatrix != 0)
                {
                    return new Vector3(valueInRow, desVec.y, valueInColumn - 1);
                }
            }
            return new Vector3(valueInRow, desVec.y, valueInColumn);
        }
        else if (dir == Vector3.right)
        {
            if (IsValidPos(valueInRow, valueInColumn + 1))
            {
                int valueMatrix = map.MapMatrix[valueInRow, valueInColumn + 1];
                if (valueMatrix != 0)
                {
                    return new Vector3(valueInRow, desVec.y, valueInColumn + 1);
                }
            }
            return new Vector3(valueInRow, desVec.y, valueInColumn);
        }
        return transform.position; //nếu hướng là vector zero thì trả về đúng vị trị đang đứng của player
    }

    private void MovingPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, desVec, speed * Time.deltaTime);
    }


    private Brick GetBrickInPlayerCurrentPos()
    {
        Vector2 posPlayerInMatrix = new Vector2(transform.position.x, transform.position.z);
        int valueInRow = Mathf.RoundToInt(posPlayerInMatrix.x);
        int valueInColumn = Mathf.RoundToInt(posPlayerInMatrix.y);
        Brick brick = map.BrickObjects[valueInRow, valueInColumn]; 
        return brick;
    }

    private void HandleBrickAndAdjustDesVecY(Brick brick)
    {
        if (brick.brickType == Brick.BrickType.Eaten || brick.brickType == Brick.BrickType.CanNotEat) return;

        Vector2 posPlayerInMatrix = new Vector2(transform.position.x, transform.position.z);
        int valueInRow = Mathf.RoundToInt(posPlayerInMatrix.x);
        int valueInColumn = Mathf.RoundToInt(posPlayerInMatrix.y);


        if (brick.brickType == Brick.BrickType.CanEat || brick.brickType == Brick.BrickType.StartPos)
        {
            map.BrickObjects[valueInRow, valueInColumn].brickType = Brick.BrickType.Eaten; 
            ownBrick++;
            listOwnBrick.Push(brick); 

            brick.transform.SetParent(transform); 
            brick.transform.localPosition = new Vector3(0, ownBrick == 1 ? 0 : ((ownBrick - 1) * -offsetBrick), 0);

            desVec.y = (ownBrick - 1) * offsetBrick; 
        }
        else if (brick.brickType == Brick.BrickType.MinusBrick)
        {
            ownBrick--;
            desVec.y = (ownBrick - 1) * offsetBrick; 
            Destroy(brick.gameObject);  

            if (listOwnBrick.Count > 0)
            {
                Brick brickRemove = listOwnBrick.Pop(); 
                brickRemove.transform.SetParent(map.transform); 
                brickRemove.transform.position = new Vector3(valueInRow, 0, valueInColumn); 
                brickRemove.SetColorBrickCanNotEat(); 
                map.BrickObjects[valueInRow, valueInColumn].brickType = Brick.BrickType.CanNotEat;
            }
            else
            {
                print("Het gach");
                enabled = false;
                Invoke(nameof(DelayLoadNewGame), 3f);
            }

        }
        else if (brick.brickType == Brick.BrickType.EndPos)
        {
            Brick[] brickRemains = transform.GetComponentsInChildren<Brick>();
            foreach (var brickChild in brickRemains)
            {
                Destroy(brickChild.gameObject);
            }
            transform.position = new Vector3(brick.transform.position.x, transform.position.y, brick.transform.position.z);

            gameObject.AddComponent<Rigidbody>();
            enabled = false;
            Invoke(nameof(DelayLoadNewMap), 3f);
        }
    }

    private bool IsValidPos(int i, int j)
    {
        return i >= 0 && i < map.MapMatrix.GetLength(0) && j >= 0 && j < map.MapMatrix.GetLength(1);
    }

    private void DelayLoadNewMap()
    {
        int currentLevel = GameManager.Instance.Level;
        currentLevel++;
        if (currentLevel > GameManager.Instance.MaxLevel)
        {
            print("U Win");
            SceneManager.LoadScene(1);
            return;
        }
        else
        {
            OnInit();
            GameManager.Instance.LoadNextLevel();
        }
    }

    private void DelayLoadNewGame()
    {
        OnInit();
        GameManager.Instance.LoadNewGame();
    }
}

