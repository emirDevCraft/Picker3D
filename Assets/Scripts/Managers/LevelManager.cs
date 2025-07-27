using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform _levelHolder;
    [SerializeField] private byte _totalLevelCount;

    private OnLevelLoaderCommand _levelLoaderCommand;
    private OnLevelDestroyerCommand _levelDestroyerCommand;

    private short _currentLevel;
    private LevelData _levelData;

    void Awake()
    {
        _currentLevel = GetActiveLevel();
        _levelData = GetLevelData();

        Init();
    }

    private void OEnable()
    {
        SubscribeEvents();
    }


    private void Init()
    {
        _levelLoaderCommand = new OnLevelLoaderCommand(_levelHolder);
        _levelDestroyerCommand = new OnLevelDestroyerCommand(_levelHolder);
    }

    public void LevelLoader(byte levelIndex)
    {
        _levelLoaderCommand.Execute(levelIndex);
    }

    public void LevelDestroyer()
    {
        _levelDestroyerCommand.Execute();
    }

    private LevelData GetLevelData()
    {
        return Resources.Load<CD_Level>("Data/CD_Level").Levels[_currentLevel];
    }

    private byte GetActiveLevel()
    {
        return (byte)_currentLevel;
    }
    private void SubscribeEvents()
    {
        CoreGameSignals.Instance.onLevelInitialize += _levelLoaderCommand.Execute;
        CoreGameSignals.Instance.onClearActiveLevel += _levelDestroyerCommand.Execute;
        CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue;
        CoreGameSignals.Instance.onNextLevel += OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
    }

    private byte OnGetLevelValue()
    {
        return (byte)_currentLevel;
    }

    private void OnNextLevel()
    {
        _currentLevel++;
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onReset?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % _totalLevelCount));
    }

    private void OnRestartLevel()
    {
        CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
        CoreGameSignals.Instance.onReset?.Invoke();
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % _totalLevelCount));
    }
    private void UnSubscribeEvents()
    {
        CoreGameSignals.Instance.onLevelInitialize -= _levelLoaderCommand.Execute;
        CoreGameSignals.Instance.onClearActiveLevel -= _levelDestroyerCommand.Execute;
        CoreGameSignals.Instance.onGetLevelValue -= OnGetLevelValue;
        CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
        CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
    }
    private void OnDisable()
    {
        UnSubscribeEvents();
    }
    private void Start()
    {
        CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % _totalLevelCount));
        //UISignals
    }
    
}