using System;
using UnityEngine;

public class WeaponUpgradeStationUI : MonoBehaviour
{
    [SerializeField] private GameObject wrapper;

    WeaponUpgradeStation station;
    IPlayerInteractor interactor;

    internal void Close()
    {
        station = null;
        interactor = null;
        wrapper.SetActive(false);
    }

    internal void Show(WeaponUpgradeStation station, IPlayerInteractor interactor)
    {
        this.station = station;
        this.interactor = interactor;
    }
}
