using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetsHealthTracker : MonoBehaviour
{
    [SerializeField] private float checkCooldown = 1f;

    private readonly List<IDamagable> healthProviders = new();

    private float currentCheckTime;
    private float healthTrackDistance = 10f;
    Transform self;

    public void Init(Transform self)
    {
        this.self = self;   
    }

    private void OnEnable()
    {
        SceneTransitionManager.SceneLoadedAfterTravel += OnSceneLoadAfterTravel;
        HumanoidAIDamageController.NotifySource += OnSourceNotified;
    }

    private void OnDisable()
    {
        SceneTransitionManager.SceneLoadedAfterTravel -= OnSceneLoadAfterTravel;
        HumanoidAIDamageController.NotifySource -= OnSourceNotified;
    }


    private void OnSceneLoadAfterTravel(string arg1, Vector3 vector)
    {
        healthProviders.Clear();
    }


    private void OnSourceNotified(IDamagable target)
    {
        TryAddTarget(target);
        TrackTargeState(target);
    }



    public void TryAddTarget(IDamagable target)
    {
        if (target == null || target.HealthProviderUI == null)
            return;

        if (healthProviders.Contains(target))
            return;

        healthProviders.Add(target);
        target.HealthProviderUI.EnableUI();
    }

    public void RemoveTarget(IDamagable target)
    {
        if (target == null)
            return;

        if (target.HealthProviderUI != null)
            target.HealthProviderUI.DisableUI();

        healthProviders.Remove(target);
    }

    private void Update()
    {
        if (healthProviders.Count == 0)
            return;

        currentCheckTime += Time.deltaTime;

        if (currentCheckTime < checkCooldown)
            return;

        currentCheckTime = 0f;

        for (int i = healthProviders.Count - 1; i >= 0; i--)
        {
            var target = healthProviders[i];
            TrackTargeState(target);    
        }
    }

    void TrackTargeState(IDamagable target)
    {
        float dist = Vector3.Distance(self.transform.position, target.GetOrigin().transform.position);


        if (target.IsDead || target.IsKnockedOut || dist > healthTrackDistance)
        {
            RemoveTarget(target);
        }
    }
}