using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] CinemachineCamera cinemachine;
    [SerializeField] CameraCutout cutout;

    public void AttachCameraToPlayer(Transform target)
    {
        //var target = GameObject.FindGameObjectWithTag("CameraFollow").transform;

        cinemachine.Target.TrackingTarget = target;

        cutout.Init(target, mainCamera);
    }


    public void ResetCameraPosition()
    {
        cinemachine.PreviousStateIsValid = false;
    }
}
