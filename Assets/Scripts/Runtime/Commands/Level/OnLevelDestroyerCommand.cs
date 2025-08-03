using UnityEngine;

public class OnLevelDestroyerCommand
{
    private Transform _levelHolder;
    internal OnLevelDestroyerCommand(Transform levelHolder)
    {
        _levelHolder = levelHolder;
    }

    internal void Execute()
    {
        if (_levelHolder.childCount <= 0) return;
        Object.Destroy(_levelHolder.GetChild(0).gameObject);
    }
}