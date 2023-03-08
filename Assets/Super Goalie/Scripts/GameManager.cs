using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _endGameMenu;
    [SerializeField] private Text _Timer;

    [SerializeField] private GameObject[] _pauseItems;
    [SerializeField] private GameObject[] _gameItems;
    
    [SerializeField] private float _gameTime = 180f;
    [SerializeField] private float _panicTime = 10f;

    [SerializeField] private float _timeBetweenKicks = 5f;

    [SerializeField] private GameObject _tikTimerObj;

    [SerializeField] private GameObject _playerSpawner;
    
    private float _currentTime = 0f;
    private float _remainingTime = 0f;
    private float _startTime = 0f;
    private bool _isPaused = false;
    private bool _hasEnded = false;
    

    public static GameManager Instance
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    private void Awake()
    {
        _startTime = Time.time;
        PoolSystem.Create();
        //if(!_playerSpawner) _playerSpawner = GameObject.FindObjectOfType<PlayerController>().gameObject;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_hasEnded)
        {
            _Timer.text = "0:00";
            return;
        }
        
        _currentTime = Time.time - _startTime;
        _remainingTime = _gameTime - _currentTime;
        
        if(_remainingTime <= _panicTime && _tikTimerObj.activeSelf == false) _tikTimerObj.SetActive(true);

        if (_remainingTime <= 0)
        {
            // Game end logic
            _tikTimerObj.SetActive(false);
            EndGame();
            ShowOrHidePauseItems(true);
            _endGameMenu.SetActive(true);
            _hasEnded = true;
        }

       /* if (Input.GetButton('Pause'))
        {
            if(_playerSpawner.activeSelf) PauseGame(!_isPaused);
        }*/

        _Timer.text = ConvertTime();
        
        if ( _remainingTime <= 125 && _remainingTime >= 120) _timeBetweenKicks = 3.5f;
        if (_remainingTime <= 60 && _remainingTime >= 55) _timeBetweenKicks = 2.5f;
    }

    public bool HasStarted()
    {
        return _playerSpawner.activeSelf;
    }

    public void StartGame()
    {
        _startTime = Time.time;
        _playerSpawner.SetActive(true);
        ShowOrHidePauseItems(false);
    }

    private string ConvertTime()
    {
        var minutes = _remainingTime / 60f;
        var seconds = ((minutes) - Mathf.Floor(minutes));
        minutes = minutes - seconds;
        seconds = Mathf.Ceil(seconds * 60);

        return minutes.ToString() + ":" + ((seconds < 10) ? "0" + seconds.ToString() : seconds.ToString());
    }

    public bool IsEnded()
    {
        return _hasEnded;
    }

    public float NextKickTime()
    {
        return _timeBetweenKicks;
    }

    private void EndGame()
    {
        DestroyAllBalls();
        DestroyAllPlayers();
    }


    private void DestroyAllPlayers()
    {
        var players = GameObject.FindObjectsOfType<PlayerAnimator>();
        _playerSpawner.gameObject.SetActive(false);

        foreach (var player in players)
        {
            player.gameObject.SetActive(false);
        }
    }

    private void DestroyAllBalls()
    {
        var balls = GameObject.FindObjectsOfType<BallPhysics>();

        foreach (var ball in balls)
        {
            ball.gameObject.SetActive(false);
        }
    }


    public void PauseGame(bool isPaused)
    {
        _isPaused = isPaused;
        _pauseMenu.SetActive(isPaused);
        ShowOrHidePauseItems(isPaused);
        Time.timeScale = _isPaused ? 0 : 1;
    }

    private void ShowOrHidePauseItems(bool isPaused)
    {
        foreach (var pauseItem in _pauseItems)
        {
            pauseItem.SetActive(isPaused);
        }

        foreach (var gameItem in _gameItems)
        {
            gameItem.SetActive(!isPaused);
        }
    }

    public void LoadScene(int sceneId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
