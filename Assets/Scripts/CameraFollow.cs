using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        Vector3 posCam = transform.position;
        Vector3 posPlayer = player.transform.position;
        posCam = new Vector3(posPlayer.x, 0.5f, posPlayer.z) + offset;
        transform.position = posCam;
    }
}
