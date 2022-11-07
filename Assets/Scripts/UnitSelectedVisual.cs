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
            } else {
                meshRenderer.enabled = true;
            }
        }
        else
        {
            if( isSelectedPointer ) {
                image.enabled = false;
            } else {
                meshRenderer.enabled = false;
            }
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;

    }
}
