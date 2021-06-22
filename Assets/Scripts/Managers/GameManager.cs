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
    /// ��ӹ۲���
    /// </summary>
    /// <param name="observer"></param>
    public void AddObserver(IEndGameObserver observer)
    {
        endGameObservers.Add(observer);
    }

    /// <summary>
    /// �Ƴ��۲���
    /// </summary>
    /// <param name="observer"></param>
    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    /// <summary>
    /// ֪ͨ�۲���
    /// </summary>
    public void NotifyObservers()
    {
        endGameObservers.ForEach(a=>a.EndNotify());
    }


}
