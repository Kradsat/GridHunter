using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Button endTurnButton;
    [SerializeField] private GameObject enemyTurnVisualGameObject;
    [SerializeField] private GameObject[ ] TurnImage;

    int[ ] order = { 0, 1, 2, 3, 4, 5 };

    private void Start()
    {
        endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        } );

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        UpdateTurnText();
        UpdateEnemyVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyVisual();
        UpdateEndTurnButtonVisibility();
        SwitchTurnImage( );
    }

    private void UpdateTurnText()
    {
        textMeshProUGUI.text = "Turn " + TurnSystem.Instance.GetTurnNumber();

    }

    private void UpdateEnemyVisual()
    {
        enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

    private void SwitchTurnImage( ) {
        for( int i = 0; i < 6; i++ ) {
            order[ i ]--;
            if( order[ i ] < 0 ) {
                order[ i ] = 5;
            }
            TurnImage[ i ].transform.localPosition = new Vector3( 0, 352 - ( 146 * order[ i ] ), 0 );
        }
    }
}
