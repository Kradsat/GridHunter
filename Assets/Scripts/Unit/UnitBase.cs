using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : UnitStatus
{
    public bool IsEnemy
    {
        get { return base.Player_Unit.Id < 4; }
    }


    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
