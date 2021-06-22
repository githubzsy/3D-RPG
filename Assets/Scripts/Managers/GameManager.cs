using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerStats;

    List<IEndGameObserver> endGameObservers=new List<IEndGameObserver>();

    public void RegisterPlayer(CharacterStats characterStats)
    {
        playerStats = characterStats;
    }

    /// <summary>
    /// 添加观察者
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    /// <summary>
    /// 移除观察者
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    /// <summary>
    /// 通知观察者
    /// </summary>
    public void NotifyObservers()
    {
        endGameObservers.ForEach(a=>a.EndNotify());
    }


}
