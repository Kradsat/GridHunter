using UnityEngine;
using System.IO;

public class DataLoader : MonoBehaviour
{
    [Header("Json")]
    [SerializeField] private TextAsset EquipmentDataJson;
    [SerializeField] private TextAsset UnitDataJson;

    //TODO
    [Header("CSV")]
    [SerializeField] private TextAsset MapCSV;

    public void LoadAllDatas()
    {
        //json
        MasterDataContainer.Instance.UnitDatas = JsonUtility.FromJson<Unit_Datas>(UnitDataJson.text);
        MasterDataContainer.Instance.EquipmentDatas = JsonUtility.FromJson<Equipments>(EquipmentDataJson.text);

        //csv
        LoadMapData();
    }

    //今はステージ１しかロードしません
    private void LoadMapData()
    {
        StringReader reader = new StringReader(MapCSV.text);

        for (int y = 0; y < 10; y++)
        {
            string[] line = reader.ReadLine().Split(",");
            for (int x = 0; x < 10; x++)
            {
                MasterDataContainer.Instance.Map[x, y] = line[x];
            }
        }
    }

    //private void Awake()
    //{
    //    LoadAllDatas();

    //    foreach (var data in MasterDataContainer.Instance.UnitDatas.unit_datas)
    //    {
    //        Debug.Log("id: " + data.unit_id + "/name: " + data.unit_name);
    //    }

    //    foreach (var data in MasterDataContainer.Instance.EquipmentDatas.equipment_datas)
    //    {
    //        Debug.Log(data.unit_equipments[0].equipment_id);
    //        Debug.Log(data.unit_equipments[0].equipment_name);
    //        Debug.Log(data.unit_equipments[0].equipment_attack);
    //    }

    //    foreach (var data in MasterDataContainer.Instance.Map)
    //    {
    //        Debug.Log(data);
    //    }
    //}
}
