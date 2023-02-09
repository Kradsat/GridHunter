using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;

public class DataLoader : MonoBehaviour {
    [Header("Json")]
    [SerializeField] private TextAsset EquipmentDataJson;
    [SerializeField] private TextAsset UnitDataJson;

    [Header("CSV")]
    [SerializeField] private TextAsset MapCSV;
    [SerializeField] private TextAsset UnitEquipCSV;


    public void LoadAllDatas()
    {
        CheckData();

        //json
        MasterDataContainer.Instance.UnitDatas = JsonUtility.FromJson<Units>(UnitDataJson.text);
        MasterDataContainer.Instance.EquipmentDatas = JsonUtility.FromJson<Equipments>(EquipmentDataJson.text);

        //csv
        LoadMapData();
        LoadUnitEquip();
    }

    private void CheckData()
    {
        if(EquipmentDataJson == null)
        {
            EquipmentDataJson = Resources.Load<TextAsset>("Data/Equipment_Data");
        }

        if (UnitDataJson == null)
        {
            UnitDataJson = Resources.Load<TextAsset>("Data/Unit_Data");
        }

        if (MapCSV == null)
        {
            var stage = "Stage1";
            if(SceneManager.GetActiveScene().name == "scene2")
            {
                stage = "Stage2";
            }

            UnitDataJson = Resources.Load<TextAsset>("Data/" + stage);
        }

        if (UnitEquipCSV == null)
        {
            UnitEquipCSV = Resources.Load<TextAsset>("Data/Team_Data");
        }
    }

    public void LoadMapData(Action callback = null)
    {
        StringReader reader = new StringReader(MapCSV.text);

        for (int y = 0; y < 15; y++)
        {
            string[] line = reader.ReadLine().Split(",");
            for (int x = 0; x < 15; x++)
            {
                if (!String.IsNullOrEmpty(line[x]))
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
