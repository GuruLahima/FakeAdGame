using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerationManager : MonoBehaviour
{
    [Header("Spawning parameters")]
    [SerializeField] private GameObject gate;
    [SerializeField] private GameObject basicEnemyPrefab;
    [SerializeField] private float basicEnemyRadius;
    [SerializeField] private GameObject bossEnemyPrefab;
    [SerializeField] private float bossEnemyRadius;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Quaternion startingRotation;
    [SerializeField] private int startingAmountOfUnits;
    [SerializeField] private int minAmountOfUnits;
    [SerializeField] private Range spawnFluctuationRangeX;
    [SerializeField] private Range spawnFluctuationRangeY;

    // parameters for spawning units in a grid
    [SerializeField] private Vector2 gridSpacing = new Vector2(1, 1);
    [SerializeField] private int gridSize = 4;

    [Header("Movement parameters")]
    [SerializeField] private Vector3 directionOfUnitMovement;
    [SerializeField] private float unitSpeed;


    [Header("Animation parameters")]
    [SerializeField] private float baseEnemyUnitAnimationSpeed;



    private List<GameObject> managedUnits = new List<GameObject>();

    [Serializable]
    internal struct Range
    {
        public float min;
        public float max;
    }

    // [Header("Debug parameters")]

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.GameStarted && !GameManager.Instance.GamePaused)
        {
            UnitSpawning();

            UnitMovement();

        }

    }

    #region public methods

    public void PreloadEverything()
    {
        PreloadEnemyUnits();
        // PreloadGates();
    }

    [ContextMenu("Load initial Units")]
    public void LoadInitialUnits()
    {

        UnitSpawning();

    }

    public void LoadInitialGates()
    {
        // throw new NotImplementedException();
    }

    public bool IsUnitHit(Vector3 projectilePos, out GameObject unitHit)
    {

        foreach (var unit in managedUnits)
        {
            if (Vector2.Distance(new Vector2(projectilePos.x, projectilePos.z), new Vector2(unit.transform.position.x, unit.transform.position.z)) <= basicEnemyRadius)
            {
                unitHit = unit;
                return true;
            }

        }

        unitHit = null;
        return false;
    }

    public void KillUnit(GameObject unitToKill)
    {
        managedUnits.Remove(unitToKill);
        PoolManager.ReleaseObject(unitToKill);
    }

    #endregion

    #region private methods


    private void UnitSpawning()
    {
        while (managedUnits.Count < minAmountOfUnits)
        {

            var newBasicUnit = SpawnBasicUnit();

            // assign random animation starting point
            newBasicUnit.GetComponent<Animator>().Play("Charge", 0, Random.Range(0f, 1f));

            // change animation speed
            newBasicUnit.GetComponent<Animator>().speed = baseEnemyUnitAnimationSpeed;


            RegisterUnitWithEnemyMovementSystem(newBasicUnit);
        }

    }

    private GameObject SpawnBasicUnit()
    {

        Vector3 pos = GetSpawnPosition(GridSpawnWithSmallFluctuationsSpawnApproach);

        PoolManager.Instance.root = spawnTransform;
        var newBasicEnemy = PoolManager.SpawnObject(basicEnemyPrefab, pos, startingRotation);

        return newBasicEnemy;
    }

    private void RegisterUnitWithEnemyMovementSystem(GameObject gameObject)
    {
        managedUnits.Add(gameObject);
    }

    private void UnitMovement()
    {
        foreach (GameObject go in this.managedUnits)
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

    private Vector3 GridSpawnWithSmallFluctuationsSpawnApproach(int gridSize, Vector2 gridSpacing)
    {
        int row = 0;
        int collumn = 0;

        row = managedUnits.Count / gridSize;
        collumn = managedUnits.Count - (row * gridSize);


        Vector3 spawnPos = spawnTransform.position + new Vector3(collumn * gridSpacing.x, 0, row * gridSpacing.y);

        // add small fluctuation to the position so it doesn't look like a grid
        spawnPos += new Vector3(Random.Range(spawnFluctuationRangeX.min, spawnFluctuationRangeX.max), 0, Random.Range(spawnFluctuationRangeY.min, spawnFluctuationRangeY.max));

        // since I expect spawnTransform to be in a horizontal center, shift the whole thing to the left for half its width
        spawnPos -= new Vector3((float)gridSize * gridSpacing.x / 2, 0, 0);
        return spawnPos;
    }

    private Vector3 GridSpawnApproach(int gridSize, Vector2 gridSpacing)
    {
        int row = 0;
        int collumn = 0;

        row = managedUnits.Count / gridSize;
        collumn = managedUnits.Count - (row * gridSize);


        Vector3 spawnPos = spawnTransform.position + new Vector3(collumn * gridSpacing.x, 0, row * gridSpacing.y);
        // since I expect spawnTransform to be in a horizontal center, shift the whole thing to the left for half its width
        spawnPos -= new Vector3((float)gridSize * gridSpacing.x / 2, 0, 0);
        return spawnPos;
    }

    #endregion

#if UNITY_EDITOR
    [Header("Debugging stuff")]
    [SerializeField] private Color unitBase_color;
    [SerializeField] private Vector3 unitBase_box_size;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = unitBase_color;
        foreach (var unit in managedUnits)
        {
            Gizmos.DrawWireCube(unit.transform.position, unitBase_box_size);
        }
    }
#endif
}
