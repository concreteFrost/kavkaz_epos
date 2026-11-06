using Unity.Cinemachine;
using UnityEngine;

public class PlayerCameraManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    [SerializeField] CinemachineCamera cinemachine;
    [SerializeField] CameraCutout cutout;
    [SerializeField] TerrainCameraCutout terrainCutout;

    public void AttachCameraToPlayer(Transform target)
    {
        //var target = GameObject.FindGameObjectWithTag("CameraFollow").transform;

        cinemachine.Target.TrackingTarget = target;
        cutout.Init(target, mainCamera);
        terrainCutout.Init(target, mainCamera, cutout.CutoutSize, cutout.CutoutSpeed);
    }


    public void ResetCameraPosition()
    {
        cinemachine.PreviousStateIsValid = false;
    }


}
