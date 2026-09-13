using FMODUnity;
using System;
using UnityEngine;

[System.Serializable]
public class DoorState
{
    public string id;
    public bool isOpened;

    public float[] lastRotation = new float[3]; 

}

public class Door : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 3f;

    [SerializeField] Transform doorPivot;

    [SerializeField] protected EventReference ev_doorState;
    [SerializeField] protected EventReference ev_doorSlide;

    public string doorName;
    public bool isOpened;

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    [HideInInspector] public string id;

    public static Action<string> DoorMessage;


    public void Init()
    {
        id = GetComponent<UniqueId>().uniqueId;
        
        openAngle = 75f;
        closedRotation = doorPivot.rotation;
    }

    public virtual void OpenDoor(IInteractor interactor)
    {
        isOpened = true;

        closedRotation = doorPivot.localRotation;

        Vector3 toInteractor =
            interactor.InteractorPosition() - doorPivot.position;

        toInteractor.y = 0f;
        toInteractor.Normalize();

        Vector3 openDirection = -toInteractor;

        float side = Vector3.Dot(doorPivot.forward, openDirection);
        float direction = side >= 0f ? 1f : -1f;

        openedRotation = closedRotation * Quaternion.Euler(
            0f,
            openAngle * direction,
            0f
        );

        AudioEventPlayer.Play3DOneShot(ev_doorSlide, gameObject);

        StartCoroutine(OpenDoorRoutine());
    }

    public void SendDoorMessage(string msg)
    {
        DoorMessage?.Invoke(msg);
    }

    public void PlayLockedEvent()
    {
        AudioEventPlayer.Play3DOneShot(ev_doorState, gameObject, "DoorState", 0);
    }

    public void PlayUnlockedEvent()
    {
        AudioEventPlayer.Play3DOneShot(ev_doorState, gameObject, "DoorState", 1);
    }

    private System.Collections.IEnumerator OpenDoorRoutine()
    {
        
        while (Quaternion.Angle(doorPivot.localRotation, openedRotation) > 0.1f)
        {
            doorPivot.localRotation = Quaternion.Slerp(
                doorPivot.localRotation,
                openedRotation,
                Time.deltaTime * openSpeed
            );

            yield return null;
        }

        doorPivot.localRotation = openedRotation;
    }

    public void LoadState(DoorState state)
    {
        isOpened = state.isOpened;

        doorPivot.localEulerAngles = new Vector3(state.lastRotation[0], state.lastRotation[1], state.lastRotation[2]);
    }

    public DoorState SaveDoorState()
    {
        Vector3 rotation = doorPivot.localEulerAngles;

        return new DoorState
        {
            id = id,
            isOpened = isOpened,
            lastRotation = new float[3]
            {
            rotation.x,
            rotation.y,
            rotation.z
            }
        };
    }
}
