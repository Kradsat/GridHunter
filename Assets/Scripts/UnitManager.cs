using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField]private List<UnitBase> unitList;
    [SerializeField]private List<UnitBase> allyUnitList;
    [SerializeField]private List<UnitBase> enemyUnitList;
    [SerializeField] private GameObject Win;
    [SerializeField] private GameObject Lose;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("there is more than one UnitManager" + transform + "_" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        unitList = new List<UnitBase>();
        allyUnitList = new List<UnitBase>();
        enemyUnitList = new List<UnitBase>();
        UnitBase.OnAnyUnitSpawn += Unit_OnAnyUnitSpawn;
        UnitBase.OnAnyUnitDead += Unit_OnAnyUnitDead;
        Win.SetActive(false);
        Lose.SetActive(false);        
    }

    private void Unit_OnAnyUnitSpawn(object sender, EventArgs e)
    {
        UnitBase unit = sender as UnitBase;

        //Debug.Log(unit + "spawned");

        unitList.Add(unit);

        if (unit.IsEnemy)
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            allyUnitList.Add(unit);
        }
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        UnitBase unit = sender as UnitBase;

        Debug.Log(unit + "died");

        unitList.Remove(unit);

        if (unit.IsEnemy)
        {
            enemyUnitList.Remove(unit);
            if( enemyUnitList.Count == 0 ) {
                Win.SetActive( true );
            }
        }
        else
        {
            allyUnitList.Remove(unit);
            if( allyUnitList.Count == 0 ) {
                Lose.SetActive( true );
            }
        }
    }

    public List<UnitBase> GetUnitList()
    {
        return unitList;
    }
    public List<UnitBase> GetAllyUnitList()
    {
        return allyUnitList;
    }
    public List<UnitBase> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
}
