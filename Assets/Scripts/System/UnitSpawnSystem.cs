using UnityEngine;

public class UnitSpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] unit = new GameObject[7];
    [SerializeField] private DataLoader dataLoader;
    [SerializeField] private UnitActionSystem unitActionSystem;
    [SerializeField] private UnityActionSystemUI unityActionSystemUI;
    [SerializeField] private GridSystemVisual gridSystemVisual;

    private void Awake()
    {
        dataLoader.LoadAllDatas();
        var _mapData = MasterDataContainer.Instance.Map;
        var _unitData = MasterDataContainer.Instance.UnitDatas;

        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                var unitType = _mapData[x, y];
                if(unitType != 0)
                {
                    var spawn = Instantiate(unit[unitType - 1]);
                    spawn.transform.SetParent(transform, false);
                    spawn.transform.position = new Vector3(x * 2, 0, y * 2);
                    var spawnUnit = spawn.GetComponent<UnitBase>();
                    if (unitType == 3)  unitActionSystem.SelectedUnit = spawnUnit;
                    

                    var unitData = new UnitStruct();
                    unitData.Id = _unitData.unit_datas[unitType].unit_id;
                    unitData.Name = _unitData.unit_datas[unitType].unit_name;
                    unitData.Hp = _unitData.unit_datas[unitType].unit_hp;
                    unitData.Attack = _unitData.unit_datas[unitType].unit_attack;
                    var equip = unitType < 5 ? MasterDataContainer.Instance.UnitEquip[unitType - 1, 1] : 0;
                    unitData.Equipment = MasterDataContainer.Instance.EquipmentDatas.equipment_datas[equip];

                    spawn.GetComponent<UnitStatus>().Init(unitData);
                }
            }
        }

        unityActionSystemUI.Initialize();
        gridSystemVisual.Initialize();
    }
}
