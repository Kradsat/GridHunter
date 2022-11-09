using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private bool isSelectedPointer;

    private MeshRenderer meshRenderer;
    private Image image;
    public Material Greenmaterial;
    public Material Redmaterial;
    bool isMaterialChanged = false;

    private void Awake()
    {

        if( !isSelectedPointer ) {
            meshRenderer = GetComponent<MeshRenderer>();
            
        } else {
            image = GetComponent<Image>( );
        }
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        UpdateVisual();

        if( !isSelectedPointer ) {
            meshRenderer.material = Greenmaterial;
        }
    }

    private void Update( ) {
        if( unit.GetActionPoints( ) < 1 && !isMaterialChanged && !isSelectedPointer ) {
            meshRenderer.material = Redmaterial;
            isMaterialChanged = true;
        } else if( unit.GetActionPoints( ) > 1 && isMaterialChanged && !isSelectedPointer ) {
            meshRenderer.material = Greenmaterial;
            isMaterialChanged = false;
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            if( isSelectedPointer ) {
                image.enabled = true;
            }
        }
        else
        {
            if( isSelectedPointer ) {
                image.enabled = false;
            }
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;

    }
}
