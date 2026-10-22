using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Custom/Weapon")]
public class Weapon : ScriptableObject
{
    public int fireRate = 1;
    public float projectileSpeed = 1;
    public GameObject model;
    [Tooltip("Position of model in local space of unit")]
    public Vector3 localPosition;
    [Tooltip("Rotation of model in local space of unit")]
    public Quaternion localRotation;
    public GameObject fireFX;
    public Orientation fireFX_orientation;
    public GameObject hitFX;
    public Orientation hitFX_orientation;
    public GameObject projectileFX;
    public Orientation projectileFX_localOrientation;

    public GameObject Fire(Transform projectilesRoot, Transform weaponInstance)
    {
        PoolManager.Instance.root = projectilesRoot;
        var projectile = PoolManager.SpawnObject(projectileFX, weaponInstance.TransformPoint(projectileFX_localOrientation.position), projectileFX_localOrientation.rotation);

        return projectile;
    }

    [Serializable]
    public struct Orientation
    {
        public Vector3 position;
        public Quaternion rotation;
    }

}