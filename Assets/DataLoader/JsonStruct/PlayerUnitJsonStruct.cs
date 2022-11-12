[System.Serializable]
public class Units
{
    public Unit_Datas[] unit_datas;
}

[System.Serializable]
public class Unit_Datas
{
    public int unit_id;
    public string unit_name;
    public int unit_hp;
    public int unit_attack;
}