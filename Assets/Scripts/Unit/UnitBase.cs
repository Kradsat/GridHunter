using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : UnitStatus
{
    public bool IsEnemy { get { return base.Unit.Id < 5; } }



    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
