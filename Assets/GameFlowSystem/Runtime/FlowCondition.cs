using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowCondition : FlowBase
{
    protected List<Func<bool>> rules;

    public FlowCondition()
    {
    }
    
    public virtual void AddRule(Func<bool> rule)
    {
        if(rules == null) rules = new List<Func<bool>>();
        rules.Add(rule);
    }

    protected virtual bool CheckRuleAllTrue() 
    {
        if(rules == null || rules.Count == 0) return true;

        var pass = true;

        foreach (var rule in rules)
        {
            if (rule.Invoke() == false)
            { 
                pass = false;
                break;
            }
        }

        return pass;
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
        if(CheckRuleAllTrue())
            holder.PlayNext();
    }
}
