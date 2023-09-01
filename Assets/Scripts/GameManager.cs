using System.Collections.Generic;
using UnityEngine;

// Observer interface
public interface IGameObserver
{
    void OnGamePaused();
    void OnGameResumed();
}

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager instance;

    // List of observers
    public List<IGameObserver> observers = new List<IGameObserver>();


    public bool isGamePaused = false;
    public bool isInputDisabled = false;
    // Input axes and buttons for controlling Player
    public string jumpButton { get; private set; }
    public string crouchButton { get; private set; }
    public string zoomButton { get; private set; }
    public string sprintButton { get; private set; }
    public string pickUpButton { get; private set; }
    public string dropButton { get; private set; }
    public string switchItemButton { get; private set; }
    public string readNotesButton { get; private set; }
    public string useItemButton { get; private set; }
    public string interactButton { get; private set; }
    public string openJournalButton { get; private set; }
    public string browseJournalButton { get; private set; }
    public string openVoiceRecordingsButton { get; private set; }
    public string horizontalAxis { get; private set; }
    public string verticalAxis { get; private set; }
    public string cameraHorizontal { get; private set; }
    public string cameraVertical { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        //Ui input that should NOT be disabled
        readNotesButton = "Read Notes";
        openJournalButton = "Open Journal";
        openVoiceRecordingsButton = "Open Voice Recordings";
        browseJournalButton = "Browse Journal";


        jumpButton = "Jump";
        crouchButton = "Crouch";
        horizontalAxis = "Horizontal";
        verticalAxis = "Vertical";
        cameraHorizontal = "Mouse X";
        cameraVertical = "Mouse Y";
        zoomButton = "Zoom";
        sprintButton = "Sprint";
        pickUpButton = "Pick Up";
        dropButton = "Drop";
        switchItemButton = "Switch Items";
        useItemButton = "Use Item";
        interactButton = "Interact";

    }

    private void Update()
    {

        if (isInputDisabled)
        {
            // Set empty string when the game is paused
            jumpButton = "";
            crouchButton = "";
            horizontalAxis = "";
            verticalAxis = "";
            cameraHorizontal = "";
            cameraVertical = "";
            zoomButton = "";
            sprintButton = "";
            pickUpButton = "";
            dropButton = "";
            switchItemButton = "";
            useItemButton = "";
            interactButton = "";
        }
        else
        {
            // Initialize input settings when the game is not paused
            jumpButton = "Jump";
            crouchButton = "Crouch";
            horizontalAxis = "Horizontal";
            verticalAxis = "Vertical";
            cameraHorizontal = "Mouse X";
            cameraVertical = "Mouse Y";
            zoomButton = "Zoom";
            sprintButton = "Sprint";
            pickUpButton = "Pick Up";
            dropButton = "Drop";
            switchItemButton = "Switch Items";
            useItemButton = "Use Item";
            interactButton = "Interact";
        }


    }

    public void AddObserver(IGameObserver observer)
    {
        if (!observers.Contains(observer))
        {
            observers.Add(observer);
            Debug.Log("Observer added: " + observer.GetType().Name);

        }
    }

    // Remove observer from the list
    public void RemoveObserver(IGameObserver observer)
    {
        observers.Remove(observer);
    }

    // Pause the game
    public void PauseGame()
    {
        NotifyObserversPaused();
        isGamePaused = true;
        isInputDisabled = true;
        Time.timeScale = 0;
        
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (var item in audioSources)
        {
            item.Pause();
        }
    }

    // Resume the game
    public void ResumeGame()
    {
        NotifyObserversResumed();
        isGamePaused = false;
        isInputDisabled = false;
        Time.timeScale = 1;

        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (var item in audioSources)
        {
            item.UnPause();
        }
    }

    // Notify all observers when the game is paused or resumed
    private void NotifyObserversPaused()
    {
        foreach (IGameObserver observer in observers)
        {
            observer.OnGamePaused();
        }
    }
    private void NotifyObserversResumed()
    {
        foreach (IGameObserver observer in observers)
        {
            observer.OnGameResumed();
        }
    }
}
