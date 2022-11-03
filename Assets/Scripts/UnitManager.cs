using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField]private List<Unit> unitList;
    [SerializeField]private List<Unit> allyUnitList;
    [SerializeField]private List<Unit> enemyUnitList;
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

        unitList = new List<Unit>();
        allyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawn += Unit_OnAnyUnitSpawn;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        Win.SetActive(false);
        Lose.SetActive(false);
    }

    private void Unit_OnAnyUnitSpawn(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        Debug.Log(unit + "spawned");

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
        Unit unit = sender as Unit;

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

    public List<Unit> GetUnitList()
    {
        return unitList;
    }
    public List<Unit> GetAllyUnitList()
    {
        return allyUnitList;
    }
    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }
}
