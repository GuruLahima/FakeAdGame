using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    [SerializeField] private LevelGenerationManager levelGenerationManager;



    private bool gamePaused = false;
    private bool gameStarted = false;
    public bool GameStarted { get { return gameStarted; } }
    public bool GamePaused { get { return gamePaused; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            DestroyImmediate(this);
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void PauseGame()
    {
        this.gamePaused = true;
    }

    public void UnpauseGame()
    {
        this.gamePaused = false;
    }

    public void StartGame()
    {
        this.gameStarted = true;
        this.gamePaused = false;
    }

    public void LoadGame()
    {
        levelGenerationManager.LoadInitialUnits();
        levelGenerationManager.LoadInitialGates();
    }
}
