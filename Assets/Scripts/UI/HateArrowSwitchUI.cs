using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HateArrowSwitchUI : MonoBehaviour {
    [SerializeField] private GameObject _targetArrowParent;
    [SerializeField] private bool _isActive;

    void Start()
    {
        _isActive = false;
        _targetArrowParent.SetActive(_isActive);
    }

    public void OnSwitchButtonClick()
    {
        _isActive = !_isActive;
        _targetArrowParent.SetActive(_isActive);
    }
}
