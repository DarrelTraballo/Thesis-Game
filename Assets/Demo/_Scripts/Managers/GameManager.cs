using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    // Singleton reference
    public static GameManager Instance { get; private set; }
    private void Awake() 
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else 
            Instance = this;

    }

    [Header("Levels")]
    public List<LevelData> levels;

    [Header("Level Info")]
    public int currentLevelIndex = 1;
    public TextMeshProUGUI txtMissionUpdate;
    public TextMeshProUGUI txtInteractMessage;
    public TextMeshProUGUI crossHairText;

    [Header("Level Elements")]
    public bool isDoorUnlocked;
    public Player Player { get; private set; }
    private GameObject currentLevelPrefab;

    private void Start() 
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SetCursorState(CursorLockMode.Locked);
        LoadLevel(currentLevelIndex);
    }

    private void Update() 
    {
        
    }

    // TODO: LEVEL 4
    //          NEED TO COMPLETE MULTIPLE PUZZLES/COLLECT N KEYS FOR DOOR TO UNLOCK

    public void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Count)
        {
            Debug.Log("No more levels to load");
            return;
        }

        Destroy(currentLevelPrefab);
        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= levels.Count) 
        {
            Debug.Log("No more levels to load");
            return;
        }
        // GameObject player = GameObject.FindGameObjectWithTag("Player");

        var levelToLoad = levels[levelIndex];
        currentLevelPrefab = Instantiate(levelToLoad.levelPrefab);
        isDoorUnlocked = levelToLoad.isDoorUnlocked;

        if (Player == null) 
        {
            Debug.Log("no player found");
            return;
        }

        Player.transform.SetPositionAndRotation(levelToLoad.playerPosition, Quaternion.identity);
    }

    public void SetCursorState(CursorLockMode cursorLockMode)
    {
        Cursor.lockState = cursorLockMode;
        switch (cursorLockMode)
        {
            case CursorLockMode.Locked:
                Cursor.visible = false;
                break;

            case CursorLockMode.Confined:
                Cursor.visible = true;
                break;

            default:
                break;
        }
    }
}
