using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    private GameController _instance;
    private EntityController _entityController;
    [SerializeField] private ScaleController _scaleController;
    [SerializeField] private GameObject _pauseMenu;

    [SerializeField] private HealthLightManager _allyHealthLights;
    [SerializeField] private HealthLightManager _enemyHealthLights;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // Get components
        _entityController = GetComponent<EntityController>();

        // Init components
        _entityController.Init();
        _scaleController.StartCalculation();
        _allyHealthLights.InitHealthLights();
        _enemyHealthLights.InitHealthLights();
    }

    private void Update()
    {
        _entityController.HandleUpdate(_scaleController.GetAngle());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}