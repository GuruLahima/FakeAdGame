using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AttackingSystem : MonoBehaviour
{
    /// <summary>
    /// Handles attacking. Spawns and handles weapons.
    /// </summary>

    [SerializeField] private Weapon weapon;
    [SerializeField] private Transform projectilesSpawnTransform;
    [SerializeField] private Vector3 projectileMovementDirection;
    [SerializeField] private LevelGenerationManager levelGenerationManager;

    [SerializeField] public float recycleDistance;

    private List<GameObject> managedWeaponInstances = new List<GameObject>();
    private List<GameObject> managedProjectileInstances = new List<GameObject>();
    private float fireRateTimer;

    #region unity callbacks

    private void Start()
    {
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
            ScanForHits();
            RecycleProjectiles();
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

    private void ScanForHits()
    {
        GameObject unitHit;
        for (int i = 0; i < managedProjectileInstances.Count; i++)
        {
            var projectile = managedProjectileInstances[i];

            if (levelGenerationManager.IsUnitHit(projectile.transform.position, out unitHit))
            {
                levelGenerationManager.KillUnit(unitHit);
                RecycleProjectile(projectile);
                i--;

            }
        }
    }

    private void RecycleProjectiles()
    {

        for (int i = 0; i < managedProjectileInstances.Count; i++)
        {

            if (managedProjectileInstances[i].transform.position.z >= recycleDistance)
            {
                RecycleProjectile(managedProjectileInstances[i]);
                i--;
            }
            else
            {
                break;
            }
        }
    }

    private void RecycleProjectile(GameObject projectile)
    {
        PoolManager.ReleaseObject(projectile);
        managedProjectileInstances.Remove(projectile);

        Debug.Log("recycled a projectile");

    }

#if UNITY_EDITOR
    [Header("Debugging stuff")]
    [SerializeField] private Color recycler_color;
    [SerializeField] private Vector2 recycler_label_pos;
    [SerializeField] private string recycler_label_text;
    [SerializeField] private Vector2 recycler_box_pos;
    [SerializeField] private Vector3 recycler_box_size;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = recycler_color;
        Handles.Label(new Vector3(recycler_label_pos.x, recycler_label_pos.y, recycleDistance), recycler_label_text);
        Gizmos.DrawCube(new Vector3(recycler_box_pos.x, recycler_box_pos.y, recycleDistance), new Vector3(recycler_box_size.x, recycler_box_size.y, recycler_box_size.z));

    }
#endif

    #endregion
}