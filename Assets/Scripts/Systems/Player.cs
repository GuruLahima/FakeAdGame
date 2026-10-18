using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Spawning parameters")]
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Quaternion startingRotation;
    [SerializeField] private int startingAmountOfUnits;


    [Header("Movement parameters")]
    [SerializeField] private Vector3 directionOfUnitMovement;
    [SerializeField] private float unitSpeed;

    private List<GameObject> controlledUnits = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void MoveUnits(float direction)
    {

        foreach (var unit in controlledUnits)
        {
            unit.transform.Translate(directionOfUnitMovement * unitSpeed * direction);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
