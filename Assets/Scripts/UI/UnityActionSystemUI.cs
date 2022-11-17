using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnityActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionMoveButtonContainerTransform;
    [SerializeField] private Transform actionATKButtonContainerTransform;
    [SerializeField] private TextMeshProUGUI actionPointText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    // Start is called before the first frame update
    public void Initialize()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChange;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChange;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UnitBase.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        UpdateActionPoints();
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionMoveButtonContainerTransform )
        {
            Destroy(buttonTransform.gameObject);
        }

        foreach( Transform buttonTransform in actionATKButtonContainerTransform ) {
            Destroy( buttonTransform.gameObject );
        }

        actionButtonUIList.Clear();

        UnitBase selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
           if( baseAction.GetActionName( ) == "Move" && selectedUnit.GetActionPoints( ) == 2
                || baseAction.GetActionName( ) == "Stay" && selectedUnit.GetActionPoints( ) == 2 ) {
                Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionMoveButtonContainerTransform );
                ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                actionButtonUI.SetBaseAction(baseAction);
                actionButtonUIList.Add(actionButtonUI);
            } else if( baseAction.GetActionName( ) == "Attack" && selectedUnit.GetActionPoints( ) < 2
                || baseAction.GetActionName( ) == "Heal Ally" && selectedUnit.GetActionPoints( ) < 2 ) {
                Transform actionButtonTransform = Instantiate( actionButtonPrefab, actionATKButtonContainerTransform );
                ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>( );
                actionButtonUI.SetBaseAction( baseAction );
                actionButtonUIList.Add( actionButtonUI );
            }

        }
    }

    private void UnitActionSystem_OnSelectedUnitChange(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
    }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
    {
        UpdateActionPoints();
        CreateUnitActionButtons( );

    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
        {
            actionButtonUI.UpdateSelectedVisual();
        }
    }

    private void UpdateActionPoints()
    {
        UnitBase selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        actionPointText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }


}
