using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Anything that should do something when the world switches should implement this interface
/// </summary>
public interface IWorldSwitcher {
    public void OnSwitchWorld(bool firstWorldActive);
}
