using UnityEngine;
using System.IO;
using System;

public class DataLoader : MonoBehaviour {
    [Header("Json")]
    [SerializeField] private TextAsset EquipmentDataJson;
    [SerializeField] private TextAsset UnitDataJson;

    [Header("CSV")]
    [SerializeField] private TextAsset MapCSV;
    [SerializeField] private TextAsset UnitEquipCSV;


    public void LoadAllDatas()
    {
        //json
        MasterDataContainer.Instance.UnitDatas = JsonUtility.FromJson<Units>(UnitDataJson.text);
        MasterDataContainer.Instance.EquipmentDatas = JsonUtility.FromJson<Equipments>(EquipmentDataJson.text);

        //csv
        LoadMapData();
        LoadUnitEquip();
    }

    //今はステージ１しかロードしません
    public void LoadMapData(Action callback = null)
    {
        StringReader reader = new StringReader(MapCSV.text);

        for (int y = 0; y < 10; y++)
        {
            string[] line = reader.ReadLine().Split(",");
            for (int x = 0; x < 10; x++)
            {
                MasterDataContainer.Instance.Map[x, y] = int.Parse(line[x]);
            }
        }

        callback?.Invoke();
    }

    public void LoadUnitEquip()
    {
        StringReader reader = new StringReader(UnitEquipCSV.text);

        for (int y = 0; y < 2; y++)
        {
            string[] line = reader.ReadLine().Split(",");
            for (int x = 0; x < 4; x++)
            {
                MasterDataContainer.Instance.UnitEquip[x, y] = int.Parse(line[x]);
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
