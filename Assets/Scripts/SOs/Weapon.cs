using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Custom/Weapon")]
public class Weapon : ScriptableObject
{
    public int fireRate = 1;
    public GameObject model;
    [Tooltip("Position of model in local space of unit")]
    public Vector3 localPosition;
    [Tooltip("Rotation of model in local space of unit")]
    public Quaternion localRotation;
    public GameObject fireFX;
    public GameObject hitFX;
    public GameObject projectileFX;
    // public int damage = 1; damage is always 1 because health wise all enemies have 1 health

}