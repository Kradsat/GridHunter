using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UnityActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;
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
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

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
        foreach (Transform buttonTransform in actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray()) 
        {
           Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
            ActionButtonUI actionButtonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
            actionButtonUI.SetBaseAction(baseAction);

            actionButtonUIList.Add(actionButtonUI);
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
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        actionPointText.text = "Action Points: " + selectedUnit.GetActionPoints();
    }

    
}
