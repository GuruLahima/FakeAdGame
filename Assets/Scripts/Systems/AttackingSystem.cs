using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class AttackingSystem : MonoBehaviour
{
    /// <summary>
    /// Handles attacking. Spawns and handles weapons.
    /// </summary>

    [SerializeField] private Weapon weapon;


    private List<GameObject> managedWeaponInstances = new List<GameObject>();
    private Player player;

    #region unity callbacks

    private void Start()
    {

        player = GetComponent<Player>();

    }

    private void OnEnable()
    {
        Player.UnitsListUpdate += SpawnWeaponModelForUnit;
    }

    private void OnDisable()
    {
        Player.UnitsListUpdate -= SpawnWeaponModelForUnit;
    }

    private void Update()
    {
        if (GameManager.Instance.GameStarted && !GameManager.Instance.GamePaused)
        {
            ShootFromAllUnits();
        }

    }

    #endregion


    #region private methods

    private void SpawnWeaponModelForUnit(GameObject unit)
    {

        PoolManager.Instance.root = unit.transform;
        var newWeaponInstance = PoolManager.SpawnObject(weapon.model, unit.transform.TransformPoint(weapon.localPosition), unit.transform.rotation * weapon.localRotation);

    }

    private void RegisterWeaponInstance(GameObject go)
    {
        managedWeaponInstances.Add(go);
    }

    private void ShootFromAllUnits()
    {
        /* // note: possible approaches:
        * 1. by individual weapon (we iterate through every weapon and fire raycast/projectile)
        * 2. by group of units (instead of individually, we calculate bounding box/geometry of all player
        * units and use it as the basis for attacking calculations like area of attack)
        */
        foreach (var weapon in managedWeaponInstances)
        {


        }
    }

    #endregion
}