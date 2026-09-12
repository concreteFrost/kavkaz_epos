using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private float openAngle = 90f;
    [SerializeField] private float openSpeed = 3f;

    public bool isOpened;
    public bool isLocked;

    private Quaternion closedRotation;
    private Quaternion openedRotation;


    private void Start()
    {
        openAngle = 75f;
    }

    public void OpenDoor(IInteractor interactor)
    {
        Debug.Log("opening");
        isOpened = true;

        closedRotation = transform.rotation;

        Vector3 toInteractor =
            interactor.InteractorPosition() - transform.position;

        toInteractor.y = 0f;
        toInteractor.Normalize();

        // Направление от игрока
        Vector3 openDirection = -toInteractor;

        // Проецируем на локальную плоскость двери
        float side = Vector3.Dot(transform.forward, openDirection);

        float direction = side >= 0f ? 1f : -1f;

        openedRotation = closedRotation * Quaternion.Euler(
            0f,
            openAngle * direction,
            0f
        );

        StartCoroutine(OpenDoorRoutine());
    }

    private System.Collections.IEnumerator OpenDoorRoutine()
    {
        while (Quaternion.Angle(transform.rotation, openedRotation) > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                openedRotation,
                Time.deltaTime * openSpeed
            );

            yield return null;
        }

        transform.rotation = openedRotation;
    }
}
