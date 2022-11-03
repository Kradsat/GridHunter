[System.Serializable]
public class Units
{
    public Unit_Datas[] unit_datas;
}

[System.Serializable]
public class Unit_Datas
{
    public int unit_id { get; set; }
    public string unit_name { get; set; }
    public int unit_hp { get; set; }
    public int unit_attack { get; set; }
}
