using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public LevelManager currentLevel;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
