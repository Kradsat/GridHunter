using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private UnitBase selectedUnit;
    [SerializeField] private Transform _arrowParent;
    [SerializeField] private GameObject _targetArrow;
    [SerializeField] private List<GameObject> _arrowList;
    public UnitBase SelectedUnit
    {
        set {
            selectedUnit = value;
            SetSelectedUnit(selectedUnit);
        }
    }
    [SerializeField] private LayerMask unitLayerMask;

    private BaseAction selectedAction;
    private bool isBusy;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("ther is more than ine UnitActionSystem" + transform + "_"+Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start( ) {
        //SetSelectedUnit( selectedUnit );
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (isBusy)
        {
            return;
        }
        if (!TurnSystem.Instance.IsPlayerTurn)
        {
            return;
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return ;
        }

        if(TryHandleUnitSelection())
        {
            return;
        }

        HandleSelectedAction();
    }

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
           GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }

            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction))
            {
                return;
            }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);

            ///Note for myself (or anyone else who see this code)

            //The region below is another way to make actions work, but decided to use the generic
            //Take action method
            #region Switch Method

            //switch (selectedAction)
            //{
            //    case MoveAction moveAction:
            //        //moveAction.Move();
            //        SetBusy();
            //        moveAction.Move(mouseGridPosition, ClearBusy);
            //        break;
            //    case SpinAction spinAction:
            //        //spinAction.Spin();
            //        SetBusy();
            //        spinAction.Spin(ClearBusy);
            //        break;
            //    case AttackAction attackAction:
            //        SetBusy();
            //        attackAction.Attack(mouseGridPosition, ClearBusy);
            //        break;

            //}
            //OnActionStarted?.Invoke(this, EventArgs.Empty);
            #endregion
            //but maybe in the future can be useful and can migrate to the switch method
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this,isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
        AllUnitNotHavePoints( );
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out RaycastHit raycasthit, float.MaxValue, unitLayerMask))
            {
                if(raycasthit.transform.TryGetComponent<UnitBase>(out UnitBase unit))
                {
                    if (unit == selectedUnit ||
                        GetSelectedAction() == selectedUnit.GetAction<SpecialAttack>() && selectedUnit.GetActionPoints( ) > 0 ||
                        unit.GetActionPoints( ) == 0 )
                    {
                        return false;
                    }

                    if (unit.IsEnemy)
                    {
                        return false;

                    }
                    SetSelectedUnit(unit);
                    return true;
                }
            }
        }
        return false;
    }

    private void SetSelectedUnit(UnitBase unit)
    {
        HideAllTargetArrow();

        selectedUnit = unit;

        if( unit.GetActionPoints( ) == 2 ) {
            SetSelectedAction(unit.GetAction<MoveAction>());
        } else {
            foreach( BaseAction baseAction in selectedUnit.GetBaseActionArray( ) ) {
                if( baseAction.GetActionName( ) == "Attack" ) {
                    SetSelectedAction( unit.GetAction<MoveAction>( ) );
                } else if( baseAction.GetActionName( ) == "Heal Ally") {
                    SetSelectedAction( unit.GetAction<SpecialAttack>( ) );
                }

            }
        }

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        ShowTargetArrow();
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public UnitBase GetSelectedUnit()
    {
        return selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return selectedAction;
    }

    private void TurnSystem_OnTurnChanged( object sender, EventArgs e ) {
        if( TurnSystem.Instance.IsPlayerTurn ) {
            SetSelectedAction( selectedUnit.GetAction<MoveAction>( ) );
            ShowTargetArrow();
        }
        else
        {
            HideAllTargetArrow();
        }
    }

    public void ShowTargetArrow()
    {
        var enemyList = UnitManager.Instance.GetEnemyActionList();
        foreach (var enemy in enemyList)
        {
            if (enemy.Unit.Id == (int)MapData.OBJ_TYPE.NEST)
            {
                continue;
            }

            if (enemy.Target == selectedUnit)
            {
                if (selectedUnit == null)
                {
                    continue;
                }
                var arrow = Instantiate(_targetArrow, _arrowParent);
                arrow.GetComponent<TargetArrowController>().ShowArrow(enemy.transform.position, selectedUnit.transform.position);
                _arrowList.Add(arrow);
            }
        }
    }

    public void HideAllTargetArrow()
    {
        foreach (Transform child in _arrowParent)
        {
            Destroy(child.gameObject);
        }

        _arrowList.Clear();
    }

    public void AllUnitNotHavePoints( ) {
        var unitlist = UnitManager.Instance.GetAllyUnitList( );
        foreach( var unit in unitlist) {
            if ( unit.GetActionPoints( ) == 0 ) {
                continue;
            } else {
                return;
            }
        }
        TurnSystem.Instance.NextTurn( );
    }
}
