using System.Collections;
using UnityEngine;

public class ProjectileDebris : MonoBehaviour
{
    [SerializeField] float lifetime = 3f;
    [SerializeField] float scatterSpeed = 3f;

    public void TrySpawn()
    {
        var rbs = GetComponentsInChildren<Rigidbody>();

        foreach (var rb in rbs)
        {
            rb.isKinematic = false;
            rb.linearVelocity =
                Random.insideUnitSphere * scatterSpeed +
                Vector3.up * scatterSpeed;
        }

        StartCoroutine(CleanupCoroutine());
    }

    IEnumerator CleanupCoroutine()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}