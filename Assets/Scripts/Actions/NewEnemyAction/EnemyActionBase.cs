using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemyActionBase
{
    public enum AttackMode
    {
        HP_HIGH = 0,
        HP_LOW,
        Distance,
        Job,
    }

    public enum TargetJob
    {
        NONE = 0,
        HAMMER,
        SWORD,
        LANCE,
        ROD,
    }
}