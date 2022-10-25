[System.Serializable]
public class Equipments
{
    public Equipment_Datas[] equipment_datas;
}

[System.Serializable]
public class Equipment_Datas
{
    public int equipment_id;
    public string equipment_name;
    public int equipment_attack;
}
