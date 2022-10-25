using UnityEngine;

public class PlayerUnitStatus : MonoBehaviour
{
    private PlayerUnit _player_unit = new PlayerUnit();

    public void Init(PlayerUnit playerUnit)
    {
        _player_unit = playerUnit;

        Debug.Log(" Initiaized -> id:" + _player_unit.Id + " name:" + _player_unit.Name
                  + " hp:" + _player_unit.Hp + " mp:" + _player_unit.Mp
                  + " range:" + _player_unit.Range + " attack:" + _player_unit.Attack
                  + " equip:" + _player_unit.Equipment.equipment_name);

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
