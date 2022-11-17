using UnityEngine;

public class UnitSpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject[] unit = new GameObject[7];
    [SerializeField] private DataLoader dataLoader;
    [SerializeField] private UnitActionSystem unitActionSystem;
    [SerializeField] private UnityActionSystemUI unityActionSystemUI;
    [SerializeField] private GridSystemVisual gridSystemVisual;

    private const int GRID_SIZE = 2;

    private void Awake()
    {
        dataLoader.LoadAllDatas();
        var _mapData = MasterDataContainer.Instance.Map;
        var _unitData = MasterDataContainer.Instance.UnitDatas;

        for (int z = 0; z < 10; z++)
        {
            for (int x = 0; x < 10; x++)
            {
                var unitType = _mapData[x, z];
                if(unitType != 0)
                {
                    var spawn = Instantiate(unit[unitType - 1]);
                    spawn.transform.SetParent(transform, false);
                    float xPos = x * GRID_SIZE;
                    float zPos = z * GRID_SIZE;
                    if(unitType == (int)MapData.OBJ_TYPE.BOSS)
                    {
                        xPos = (x + 1.5f) * GRID_SIZE;
                        zPos = (z + 1.5f) * GRID_SIZE;
                    }
                    spawn.transform.position = new Vector3(xPos, 0, zPos);
                    var spawnUnit = spawn.GetComponent<UnitBase>();
                    if (unitType == 3)  unitActionSystem.SelectedUnit = spawnUnit;
                    

                    var unitData = new UnitStruct();
                    unitData.Id = _unitData.unit_datas[unitType].unit_id;
                    unitData.Name = _unitData.unit_datas[unitType].unit_name;
                    unitData.Hp = _unitData.unit_datas[unitType].unit_hp;
                    unitData.Attack = _unitData.unit_datas[unitType].unit_attack;
                    var equip = unitType < 5 ? MasterDataContainer.Instance.UnitEquip[unitType - 1, 1] : 0;
                    unitData.Equipment = MasterDataContainer.Instance.EquipmentDatas.equipment_datas[equip];

                    spawnUnit.Init(unitData);
                }
            }
        }

        unityActionSystemUI.Initialize();
        gridSystemVisual.Initialize();
        UnitManager.Instance.SetUnitListToEnemy();
    }
}
