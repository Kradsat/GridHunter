using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    private UnitStruct _unit = new UnitStruct();
    public UnitStruct Unit
    {
        get { return _unit; }
    }

    public virtual double HP
    {
        get { return _unit.Hp; }
        set { _unit.Hp += value; }
    }

    public virtual double ATK
    {
        get { return _unit.Attack; }
    }

    public virtual void Init(UnitStruct unit)
    {
        _unit = unit;
        /*Debug.Log(" Initiaized -> id:" + _player_unit.Id + " name:" + _player_unit.Name
                  + " hp:" + _player_unit.Hp + " attack:" + _player_unit.Attack
                  + " equip:" + _player_unit.Equipment.equipment_name);
        */
    }

}
