using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    [SerializeField]private List<UnitBase> unitList;
    [SerializeField]private List<UnitBase> allyUnitList;
    [SerializeField]private List<UnitBase> enemyUnitList;
    [SerializeField]private List<EnemyAction> enemyActionList;
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

        unitList.Add(unit);

        if (unit.IsEnemy)
        {
            enemyUnitList.Add(unit);
            enemyActionList.Add(unit.GetComponent<EnemyAction>());
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
            if ( enemyUnitList.Count == 0 ) {
                if( Win == null ) {
                    return;
                }
                Win.SetActive( true );
            }
        }
        else
        {
            allyUnitList.Remove(unit);
            if ( allyUnitList.Count == 0 ) {
                if( Lose == null ) {
                    return;
                }
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

    public List<EnemyAction> GetEnemyActionList()
    {
        return enemyActionList.OrderBy(_ => _.Unit.Id).ToList();
    }

    public void RemoveEnemyAction(EnemyAction enemy)
    {
        enemyActionList.Remove(enemy);
    }
}
