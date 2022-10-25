using UnityEngine;

public class UnitSpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] unit = new GameObject[5];
    [SerializeField] private DataLoader dataLoader;
    [SerializeField] private UnitActionSystem unitActionSystem;
    [SerializeField] private UnityActionSystemUI unityActionSystemUI;
    [SerializeField] private GridSystemVisual gridSystemVisual;

    // TODO: シーンロードする時に処理
    private void Awake()
    {
        dataLoader.LoadAllDatas();
        var _mapData = MasterDataContainer.Instance.Map;
        var _unitData = MasterDataContainer.Instance.UnitDatas;
        for (int y = 0; y < 10; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                switch (_mapData[x, y])
                {
                    case (int)MapData.OBJ_TYPE.NONE:
                        break;
                    case (int)MapData.OBJ_TYPE.DAGGER:
                    case (int)MapData.OBJ_TYPE.HAMMER:
                    case (int)MapData.OBJ_TYPE.LANCE:
                    case (int)MapData.OBJ_TYPE.ROD:
                        var unitType = _mapData[x, y];
                        Debug.Log("UnitType: " + unitType);
                        var spawn = Instantiate(unit[unitType - 1]);
                        spawn.transform.SetParent(transform, false);
                        spawn.transform.position = new Vector3(x * 2, 0, y * 2);
                        unitActionSystem.SelectedUnit = spawn.GetComponent<Unit>();
                        
                        var unitData = new PlayerUnit();
                        unitData.Id = _unitData.unit_datas[unitType].unit_id;
                        unitData.Name = _unitData.unit_datas[unitType].unit_name;
                        unitData.Hp = _unitData.unit_datas[unitType].unit_hp;
                        unitData.Mp = _unitData.unit_datas[unitType].unit_mp;
                        unitData.Range = _unitData.unit_datas[unitType].unit_range;
                        unitData.Attack = _unitData.unit_datas[unitType].unit_attack;
                        var equip = MasterDataContainer.Instance.UnitEquip[unitType - 1, 1];
                        unitData.Equipment = MasterDataContainer.Instance.EquipmentDatas.equipment_datas[equip];
                        
                        spawn.GetComponent<PlayerUnitStatus>().Init(unitData);
                        break;
                }
            }
        }

        unityActionSystemUI.Initialize();
        gridSystemVisual.Initialize();
    }
}
