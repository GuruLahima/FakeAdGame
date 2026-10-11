using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerationManager : MonoBehaviour
{
    [Header("Spawning parameters")]
    [SerializeField] private GameObject gate;
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private GameObject bossEnemyPrefab;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Quaternion startingRotation;
    [SerializeField] private int startingAmountOfUnits;
    [SerializeField] private int minAmountOfUnits;

    [Header("Movement parameters")]
    [SerializeField] private Vector3 directionOfUnitMovement;
    [SerializeField] private float unitSpeed;


    private List<GameObject> controlledUnits = new List<GameObject>();

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.GameStarted && !GameManager.Instance.GamePaused)
        {
            UnitSpawning();

            UnitLifecycle();

            UnitMovement();

        }

    }

    #region public methods

    public void LoadInitialUnits()
    {

        UnitSpawning();

    }

    public void LoadInitialGates()
    {
        // throw new NotImplementedException();
    }

    #endregion

    #region private methods


    private void UnitSpawning()
    {
        while (controlledUnits.Count < minAmountOfUnits)
        {

            var newBasicUnit = SpawnBasicUnit();

            RegisterUnitWithEnemyMovementSystem(newBasicUnit);
        }

    }

    private void UnitLifecycle()
    {
        foreach (var unit in controlledUnits)
        {
            if (!unit.gameObject.activeSelf)
            {

            }

        }
    }

    private GameObject SpawnBasicUnit()
    {

        var newBasicEnemy = PoolManager.SpawnObject(basicEnemyPrefab, spawnTransform.position, startingRotation);

        return newBasicEnemy;
    }

    private void RegisterUnitWithEnemyMovementSystem(GameObject gameObject)
    {
        controlledUnits.Add(gameObject);
    }

    private void UnitMovement()
    {
        foreach (GameObject go in this.controlledUnits)
        {
            go.transform.Translate(directionOfUnitMovement.normalized * unitSpeed * Time.deltaTime);
        }
    }



    public void PreloadEverything()
    {
        PreloadEnemyUnits();
        // PreloadGates();
    }


    private void PreloadEnemyUnits()
    {
        PoolManager.WarmPool(basicEnemyPrefab, startingAmountOfUnits);
    }

    private void PreloadGates()
    {
        PoolManager.WarmPool(gate, startingAmountOfUnits);
    }

    #endregion
}
