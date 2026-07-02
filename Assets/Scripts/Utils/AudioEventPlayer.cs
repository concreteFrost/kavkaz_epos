using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public static class AudioEventPlayer
{
    #region Play

    /// <summary>
    /// Creates and starts a 2D event.
    /// The caller is responsible for calling Stop() or Release().
    /// </summary>
    public static EventInstance Play2D(
        EventReference eventRef,
        string parameterName = null,
        float parameterValue = 0f)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventRef);

        if (!string.IsNullOrEmpty(parameterName))
            instance.setParameterByName(parameterName, parameterValue);

        instance.start();

        return instance;
    }

    /// <summary>
    /// Creates and starts a 3D event.
    /// The caller is responsible for calling Stop() or Release().
    /// </summary>
    public static EventInstance Play3D(
        EventReference eventRef,
        GameObject source,
        string parameterName = null,
        float parameterValue = 0f)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventRef);

        if (!string.IsNullOrEmpty(parameterName))
            instance.setParameterByName(parameterName, parameterValue);

        if (source != null)
        {
            RuntimeManager.AttachInstanceToGameObject(
                instance,
                source.transform,
                source.GetComponent<Rigidbody>());
        }

        instance.start();

        return instance;
    }

    /// <summary>
    /// Fire-and-forget 2D event.
    /// </summary>
    public static void Play2DOneShot(EventReference eventRef)
    {
        RuntimeManager.PlayOneShot(eventRef);
    }

    /// <summary>
    /// Fire-and-forget 3D event.
    /// </summary>
    public static void Play3DOneShot(EventReference eventRef, GameObject source)
    {
        if (source == null)
        {
            RuntimeManager.PlayOneShot(eventRef);
            return;
        }

        RuntimeManager.PlayOneShotAttached(eventRef, source);
    }

    public static EventInstance Play3DOneShot(
    EventReference eventRef,
    GameObject source,
    string parameterName,
    int parameterValue)
    {
        EventInstance instance = RuntimeManager.CreateInstance(eventRef);

        RuntimeManager.AttachInstanceToGameObject(
            instance,
            source.transform,
            source.GetComponent<Rigidbody>());

        instance.setParameterByName(parameterName, parameterValue);

        instance.start();
        instance.release();

        return instance;
    }

    #endregion

    #region Control

    public static void Stop(EventInstance instance, bool fadeOut = true)
    {
        if (!instance.isValid())
            return;

        instance.stop(fadeOut
            ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT
            : FMOD.Studio.STOP_MODE.IMMEDIATE);

        instance.release();
    }

    public static void Release(EventInstance instance)
    {
        if (!instance.isValid())
            return;

        instance.release();
    }

    public static void Pause(EventInstance instance, bool paused)
    {
        if (!instance.isValid())
            return;

        instance.setPaused(paused);
    }

    #endregion

    #region Parameters

    public static void SetParameter(
        EventInstance instance,
        string parameterName,
        float value)
    {
        if (!instance.isValid() || string.IsNullOrEmpty(parameterName))
            return;

        instance.setParameterByName(parameterName, value);
    }

    public static void SetParameterLabel(
        EventInstance instance,
        string parameterName,
        string label)
    {
        if (!instance.isValid() ||
            string.IsNullOrEmpty(parameterName) ||
            string.IsNullOrEmpty(label))
            return;

        instance.setParameterByNameWithLabel(parameterName, label);
    }

    #endregion
}