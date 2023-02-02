using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    [SerializeField] private Material grayMaterial;
    [SerializeField] private Material whiteMaterial;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material startMaterialBrick;
    [SerializeField] private Material endMaterialBrick;
    public enum BrickType
    {
        Void,
        CanEat,
        MinusBrick,
        StartPos,
        EndPos
    }
    public BrickType brickType;

    private void Start()
    {
        OnInit();
    }

    private void OnInit()
    {

        if (brickType == BrickType.Void)
        {
            GetComponent<MeshRenderer>().material = grayMaterial;
        }
        if (brickType == BrickType.CanEat)
        {
            GetComponent<MeshRenderer>().material = yellowMaterial;
        }
        if (brickType == BrickType.MinusBrick)
        {
            GetComponent<MeshRenderer>().material = whiteMaterial;
        }
        if (brickType == BrickType.StartPos)
        {
            GetComponent<MeshRenderer>().material = startMaterialBrick;
        }
        if (brickType == BrickType.EndPos)
        {
            GetComponent<MeshRenderer>().material = endMaterialBrick;
        }

    }
}
