using FMODUnity;
using System;
using System.Collections;
using UnityEngine;

#region Save Data

[Serializable]
public class DoorState
{
    public string id;
    public bool isOpened;
    public float[] lastRotation = new float[3];
}

#endregion


public class Door : MonoBehaviour
{
    #region Inspector

    [Header("Door Settings")]
    [SerializeField] private float openAngle = 75f;
    [SerializeField] private float openSpeed = 3f;
    [SerializeField] private Transform doorPivot;

    [Header("Audio")]
    [SerializeField] protected EventReference ev_doorState;
    [SerializeField] protected EventReference ev_doorSlide;

    #endregion


    #region Public Properties

    public string doorName;
    public bool isOpened;

    [HideInInspector]
    public string id;

    public static Action<string> DoorMessage;

    #endregion


    #region Private Fields

    private Quaternion closedRotation;
    private Quaternion openedRotation;

    #endregion


    #region Initialization

    public void Init()
    {
        id = GetComponent<UniqueId>().uniqueId;
        closedRotation = doorPivot.localRotation;
    }

    #endregion


    #region Door

    public virtual void OpenDoor(IInteractor interactor, float delay = 0f)
    {
        isOpened = true;

        closedRotation = doorPivot.localRotation;

        Vector3 toInteractor = interactor.InteractorPosition() - doorPivot.position;
        toInteractor.y = 0f;

        if (toInteractor.sqrMagnitude > 0.001f)
        {
            toInteractor.Normalize();
        }

        Vector3 openDirection = -toInteractor;

        float side = Vector3.Dot(doorPivot.forward, openDirection);
        float direction = side >= 0f ? 1f : -1f;

        openedRotation = closedRotation * Quaternion.Euler(
            0f,
            openAngle * direction,
            0f
        );

        

        StartCoroutine(OpenDoorRoutine(delay));
    }

    private IEnumerator OpenDoorRoutine(float delay)
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        AudioEventPlayer.Play3DOneShot(ev_doorSlide, gameObject);

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

    #endregion


    #region Events

    public void SendDoorMessage(string message)
    {
        DoorMessage?.Invoke(message);
    }

    public void PlayLockedEvent()
    {
        AudioEventPlayer.Play3DOneShot(
            ev_doorState,
            gameObject,
            "DoorState",
            0
        );
    }

    public void PlayUnlockedEvent()
    {
        AudioEventPlayer.Play3DOneShot(
            ev_doorState,
            gameObject,
            "DoorState",
            1
        );
    }

    #endregion


    #region Save / Load

    public void LoadState(DoorState state)
    {
        isOpened = state.isOpened;

        doorPivot.localEulerAngles = new Vector3(
            state.lastRotation[0],
            state.lastRotation[1],
            state.lastRotation[2]
        );
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

    #endregion
}