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

    // parameters for spawning units in a grid
    [SerializeField] private Vector2 gridSpacing = new Vector2(1, 1);
    [SerializeField] private int gridSize = 4;

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

    public void PreloadEverything()
    {
        PreloadEnemyUnits();
        // PreloadGates();
    }

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

        Vector3 pos = GetSpawnPosition(GridSpawnApproach);

        var newBasicEnemy = PoolManager.SpawnObject(basicEnemyPrefab, pos, startingRotation);

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
            go.transform.Translate(directionOfUnitMovement.normalized * unitSpeed * Time.deltaTime, Space.World);
        }
    }

    private void PreloadEnemyUnits()
    {
        PoolManager.WarmPool(basicEnemyPrefab, startingAmountOfUnits);
    }

    private void PreloadGates()
    {
        PoolManager.WarmPool(gate, startingAmountOfUnits);
    }

    private Vector3 GetSpawnPosition(Func<int, Vector2, Vector3> spawnGenerationApproach)
    {
        return spawnGenerationApproach(gridSize, gridSpacing);
    }

    private Vector3 GridSpawnApproach(int gridSize, Vector2 gridSpacing)
    {
        int row = 0;
        int collumn = 0;

        row = controlledUnits.Count / gridSize;
        collumn = controlledUnits.Count - (row * gridSize);


        Vector3 spawnPos = spawnTransform.position + new Vector3(collumn * gridSpacing.x, 0, row * gridSpacing.y);
        // since I expect spawnTransform to be in a horizontal center, shift the whole thing to the left for half its width
        spawnPos -= new Vector3((float)gridSize * gridSpacing.x / 2, 0, 0);
        return spawnPos;
    }

    #endregion
}
