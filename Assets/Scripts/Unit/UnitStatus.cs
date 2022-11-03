using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    private UnitStruct _player_unit = new UnitStruct();
    public UnitStruct Player_Unit
    {
        get { return _player_unit; }
    }

    public virtual double HP
    {
        get { return _player_unit.Hp; }
        set { _player_unit.Hp += value; }
    }

    public virtual double ATK
    {
        get { return _player_unit.Attack; }
    }

    public virtual void Init(UnitStruct unit)
    {
        _player_unit = unit;

        Debug.Log(" Initiaized -> id:" + _player_unit.Id + " name:" + _player_unit.Name
                  + " hp:" + _player_unit.Hp + " attack:" + _player_unit.Attack
                  + " equip:" + _player_unit.Equipment.equipment_name);

    }

}
