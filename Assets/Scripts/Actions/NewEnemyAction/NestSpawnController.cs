using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class NestSpawnController : MonoBehaviour
{
    private List<EnemyAction> _nest_list = new List<EnemyAction>();
    [SerializeField] private UnitSpawnSystem _unitSpawnSystem; 

    public void SpawnNewEnemy()
    {
        foreach (var nest in _nest_list) {
            _unitSpawnSystem.SpawnFromNest(nest.GridPosition);
        }
    }
}
