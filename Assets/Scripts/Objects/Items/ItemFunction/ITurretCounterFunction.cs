using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretCounterFunction
{
    public void OnFunction(TurretCounter thisCounter);
    public void Initialize();
}
