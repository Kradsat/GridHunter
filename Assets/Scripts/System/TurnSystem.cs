using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;

    private int _turnNumber = 1;
    public int TurnNumber { get { return _turnNumber; } }

    private bool _isPlayerTurn = true;
    public bool IsPlayerTurn { get { return _isPlayerTurn; } }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("ther is more than one LevelGrid" + transform + "_" + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void NextTurn() { 
        _turnNumber++;
        _isPlayerTurn = !_isPlayerTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }
}
