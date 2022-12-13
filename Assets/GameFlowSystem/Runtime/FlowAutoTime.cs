using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowAutoTime : FlowBase
{
    protected float m_Time;

    public FlowAutoTime(float time)
    {
        m_Time = time;
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
        m_Time -= Time.deltaTime;
        if (m_Time > 0.0f) return;
        holder.PlayNext();
    }
}
