using UnityEngine;

public class GameManager : MonoBehaviour
{

    //UserInterface vars
    [SerializeField] private Canvas MainGameCanvas;

    //Widget vars
    [SerializeField] private GameObject MainMenuWidget;
    [serializeField] private GameObject GameOverWidget;

    [serializeField] private GameObject PauseMenuWidget;

    // Singleton instance
    public static GameManager Instance { get; private set; }
    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        dontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
