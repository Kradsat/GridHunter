using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class SwitchtTurnBar : MonoBehaviour
{

    [ SerializeField ] private int order;

    public void SwitchPosImageTurn( ){
        int ordernumber = order + 1 - ( TurnSystem.Instance.GetTurnNumber( ) );
        if( ordernumber < 0 ) {
            ordernumber = 5;
        }
        transform.position = new Vector3 ( 0, 133 * ordernumber, 0 );
    }
}
