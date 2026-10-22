using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class AttackingSystem : MonoBehaviour
{
    /// <summary>
    /// Handles attacking. Spawns and handles weapons.
    /// </summary>

    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform projectilesSpawnTransform;
    [SerializeField] private Vector3 projectileMovementDirection;



    private List<GameObject> managedWeaponInstances = new List<GameObject>();
    private List<GameObject> managedProjectileInstances = new List<GameObject>();
    private Player player;
    private float fireRateTimer;

    #region unity callbacks

    private void Start()
    {

        player = GetComponent<Player>();
        OnWeaponUpdated();
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
            MoveProjectiles();
            // ScanForHits();
        }

    }

    #endregion

    #region public methods

    public void OnWeaponUpdated()
    {
        // this forces the system to fire the new weapon right away instead of waiting a cycle
        float fireInterval = 1 / (float)weapon.fireRate;
        fireRateTimer = fireRateTimer + 1;

    }
    #endregion


    #region private methods

    private void SpawnWeaponModelForUnit(GameObject unit)
    {

        PoolManager.Instance.root = unit.transform;
        var newWeaponInstance = PoolManager.SpawnObject(weapon.model, unit.transform.TransformPoint(weapon.localPosition), unit.transform.rotation * weapon.localRotation);
        RegisterWeaponInstance(newWeaponInstance);

    }

    private void RegisterWeaponInstance(GameObject go)
    {
        managedWeaponInstances.Add(go);
    }

    private void ShootFromAllUnits()
    {
        float fireInterval = 1 / (float)weapon.fireRate;

        fireRateTimer += Time.deltaTime;

        if (fireRateTimer >= fireInterval)
        {
            fireRateTimer = 0;

            /* // note: possible approaches:
            * 1. by individual weapon (we iterate through every weapon and fire raycast/projectile)
            * 2. by group of units (instead of individually, we calculate bounding box/geometry of all player
            * units and use it as the basis for attacking calculations like area of attack)
            */
            foreach (var weaponInstance in managedWeaponInstances)
            {
                managedProjectileInstances.Add(weapon.Fire(projectilesSpawnTransform, weaponInstance.transform));
            }
        }
    }

    private void MoveProjectiles()
    {
        foreach (var projectile in managedProjectileInstances)
        {
            projectile.transform.Translate(projectileMovementDirection * Time.deltaTime * weapon.projectileSpeed, Space.World);
        }

    }

    #endregion
}