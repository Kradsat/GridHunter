[System.Serializable]
public class Equipments
{
    public Equipment_Datas[] equipment_datas;
}

[System.Serializable]
public class Equipment_Datas
{
    public Unit_Equipments[] unit_equipments;
}

[System.Serializable]
public class Unit_Equipments
{
    public int equipment_id;
    public string equipment_name;
    public int equipment_attack;
}