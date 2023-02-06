using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    private UnitStruct _unit;
    public UnitStruct Unit
    {
        get { return _unit; }
    }

    public double MAX_HP { get; private set; }
    public virtual double HP
    {
        get { return _unit.Hp; }
        set { _unit.Hp = value; }
    }

    public virtual double ATK
    {
        get { return _unit.Attack; }
    }

    public virtual void Init(UnitStruct unit)
    {
        _unit = unit;
        MAX_HP = unit.Hp;
        //Debug.Log(" Initiaized -> id:" + _unit.Id + " name:" + _unit.Name
        //          + " hp:" + _unit.Hp + " attack:" + _unit.Attack
        //          + " equip:" + _unit.Equipment.equipment_name);
        
    }
    public bool IsEnemy { get { return _unit.Id > 4 && _unit.Id < 12; } }
}
