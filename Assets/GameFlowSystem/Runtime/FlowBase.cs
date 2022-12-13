using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowBase : IFlow
{
    public FlowBase()
    {
    }

    public override void Enter(GameFlowHolder holder)
    {
        base.Enter(holder);
    }

    public override void Exit(GameFlowHolder holder)
    {
        base.Exit(holder);
    }

    public override void Update(GameFlowHolder holder)
    {
        base.Update(holder);
    }
}
