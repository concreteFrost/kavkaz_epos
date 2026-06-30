using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake: MonoBehaviour
{

    [SerializeField] private CinemachineBasicMultiChannelPerlin noise;

    public static Action<float, float, float> Shake;


    private void Awake()
    {
        StopShake();
    }

    private void OnEnable()
    {
        Shake += OnShake;

    }

    private void OnDisable()
    {
        Shake -= OnShake;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnShake(0.5f, 1, 2);
        }
    }


    public void OnShake(float amplitude, float frequency, float duration)
    {
        StartCoroutine(ShakeCoroutine(amplitude, frequency, duration));
    }

    public void StopShake()
    {
        noise.AmplitudeGain = 0.0f;
    }


    IEnumerator ShakeCoroutine(float amplite, float frequency, float duration)
    {
        noise.AmplitudeGain = amplite;
        noise.FrequencyGain = frequency;

        float elapsed = 0;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;
            noise.AmplitudeGain = Mathf.Lerp(amplite, 0, elapsed / duration);
            yield return null;
        }

        noise.AmplitudeGain = 0f;
    }


}
