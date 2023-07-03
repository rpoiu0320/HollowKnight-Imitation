using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateBase
{
    public abstract void Setup();
    public abstract void Enter();
    public abstract void Update();
    public abstract void Transition();
    public abstract void Exit();
}
