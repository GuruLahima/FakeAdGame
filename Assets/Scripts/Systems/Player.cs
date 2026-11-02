using System;
using System.Collections.Generic;
using SimpleInputNamespace;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
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
    [SerializeField] private Vector2 movementLimits;
    [SerializeField] public Vector3 unitsCenter;

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

        if (GameManager.Instance && GameManager.Instance.GameStarted && !GameManager.Instance.GamePaused)
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


        Vector3 spawnPos = unitsCenter + new Vector3(collumn * gridSpacing.x, 0, row * gridSpacing.y);

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


#if UNITY_EDITOR
    [Header("Debugging stuff")]
    [SerializeField] private Color unitsCenter_color;
    [SerializeField] private Vector3 unitsCenter_label_offset;
    [SerializeField] private int unitsCenter_label_size;
    [SerializeField] private string unitsCenter_label_text;
    [SerializeField] private Vector3 unitsCenterBox_size;

    private void OnDrawGizmos()
    {
        Gizmos.color = unitsCenter_color;
        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = unitsCenter_label_size;
        Handles.Label(unitsCenter + unitsCenter_label_offset, unitsCenter_label_text, style);
        Gizmos.DrawCube(unitsCenter, new Vector3(unitsCenterBox_size.x, unitsCenterBox_size.y, unitsCenterBox_size.z));
    }

#endif

    #endregion
}

#if UNITY_EDITOR
[CustomEditor(typeof(Player))]
public class PlayerDebuggerEditor : Editor
{
    // Draw lines between a chosen GameObject
    // and a selection of added GameObjects

    void OnSceneGUI()
    {
        // Get the chosen GameObject
        Player t = target as Player;

        if (t == null)
            return;

        EditorGUI.BeginChangeCheck();
        // Grab the center of the parent
        Vector3 newTargetPosition = Handles.PositionHandle(t.unitsCenter, Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(t, "Change Look At Target Position");
            t.unitsCenter = newTargetPosition;
        }



    }

}
#endif
