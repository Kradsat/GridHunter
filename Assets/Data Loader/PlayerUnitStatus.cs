using UnityEngine;

public class PlayerUnitStatus : MonoBehaviour
{
    private PlayerUnit _player_unit = new PlayerUnit();

    public void Init(int id, string name, double hp, double mp, int range, int attack, int equip_id)
    {
        _player_unit.Id = id;
        _player_unit.Name = name;
        _player_unit.Hp = hp;
        _player_unit.Mp = mp;
        _player_unit.Range = range;
        _player_unit.Attack = attack;
        _player_unit.Equipment = equip_id;

        Debug.Log(" Initiaized -> id:" + _player_unit.Id + " name:" + _player_unit.Name
                  + " hp:" + _player_unit.Hp + " mp:" + _player_unit.Mp
                  + " range:" + _player_unit.Range + " attack:" + _player_unit.Attack
                  + " equip:" + _player_unit.Equipment);

    }

    //ƒ_ƒ[ƒW‚ğó‚¯‚é
    public void AddDamage(int dmg)
    { 
        _player_unit.Hp -= dmg;
    }

    //HP‰ñ•œ
    public void RecoverHP(int recover)
    {
        _player_unit.Hp += recover;
    }
}
