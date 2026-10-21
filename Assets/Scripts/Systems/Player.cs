using System;
using System.Collections.Generic;
using SimpleInputNamespace;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Events")]
    public static Action<GameObject> UnitsListUpdate;

    [Header("Spawning parameters")]
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private Quaternion startingRotation;
    [SerializeField] private int startingAmountOfUnits;


    // parameters for spawning units in a grid
    [SerializeField] private Vector2 gridSpacing = new Vector2(1, 1);
    [SerializeField] private int gridSize = 4;


    [Header("Movement parameters")]
    [SerializeField] private string horizontalAxis;
    [SerializeField] private Joystick joystick;
    [SerializeField] private Vector3 directionOfUnitMovement;
    [SerializeField] private float unitSpeed;

    private List<GameObject> managedUnits = new List<GameObject>();

    [Header("Debug")]

    [SerializeField] private float GetAxisSensitivity = 20f;

    [SerializeField] private float GetAxisDeadZone = 0.025f;

    [SerializeField] private bool GetAxisSnapValue = true;

    [SerializeField] private bool GetAxisTimeScaleDependent = true;

    // Update is called once per frame
    void Update()
    {
        SimpleInput.GetAxisSensitivity = GetAxisSensitivity;
        SimpleInput.GetAxisSnapValue = GetAxisSnapValue;
        SimpleInput.GetAxisDeadZone = GetAxisDeadZone;
        SimpleInput.GetAxisTimeScaleDependent = GetAxisTimeScaleDependent;

        if (GameManager.Instance.GameStarted && !GameManager.Instance.GamePaused)
            MoveUnits(SimpleInput.GetAxis("Horizontal"));

    }


    #region public methods

    [ContextMenu("Load initial Units")]
    public void LoadInitialUnits()
    {
        for (int i = 0; i < startingAmountOfUnits; i++)
        {
            UnitSpawning();
        }

    }

    public void MoveUnits(float input)
    {
        float direction = SignOf(input);
        foreach (var unit in managedUnits)
        {
            unit.transform.Translate(directionOfUnitMovement * unitSpeed * input * Time.deltaTime);
        }

    }

    #endregion


    #region private methods

    private void UnitSpawning()
    {

        var newBasicUnit = SpawnBasicUnit();

        RegisterUnitWithMovementSystem(newBasicUnit);

    }

    private void RegisterUnitWithMovementSystem(GameObject go)
    {
        managedUnits.Add(go);

        UnitsListUpdate?.Invoke(go);
    }

    private GameObject SpawnBasicUnit()
    {

        Vector3 pos = GetSpawnPosition(GridSpawnApproach);

        PoolManager.Instance.root = spawnTransform;
        var newBasicEnemy = PoolManager.SpawnObject(soldierPrefab, pos, startingRotation);

        return newBasicEnemy;
    }

    private Vector3 GetSpawnPosition(Func<Vector3> spawnGenerationApproach)
    {
        return spawnGenerationApproach();
    }

    private Vector3 GridSpawnApproach()
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

    private float SignOf(float num)
    {
        if (Mathf.Approximately(0f, num))
            return 0;

        return Mathf.Sign(num);
    }

    #endregion
}
