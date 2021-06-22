using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEndGameObserver
{
    /// <summary>
    /// 结束游戏时的广播
    /// </summary>
    void EndNotify();
}
