using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameFlow : MonoBehaviour
{
    public GameFlowHolder gameFlowHolder;
    void Start()
    {
        gameFlowHolder = new GameFlowHolder();

        FlowAutoTime tt = new FlowAutoTime(1);
        tt.EnterEvent += () => { print("0En"); };
        tt.ExitEvent += () => { print("0Ex"); };
        gameFlowHolder.Add(tt);

        FlowCondition cc = new FlowCondition();
        cc.ExitEvent += () => { print("1Ex"); };
        cc.AddRule(() => { return Input.GetKey(KeyCode.A); });
        cc.AddRule(() => { return Input.GetKey(KeyCode.D); });
        gameFlowHolder.Add(cc);

        gameFlowHolder.PlayNext();
    }

    void Update()
    {
        gameFlowHolder.Update();
    }
}
