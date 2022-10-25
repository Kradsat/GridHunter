using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectorManager : MonoBehaviour
{
    public void Title( ) {
        SceneManager.LoadScene( "TitleScene" );
    }
    public void StageSelector(  ) {
        SceneManager.LoadScene( "StageSelectorScene" );    
    }

    public void Stage1( ) {
        SceneManager.LoadScene( "GameScene" );
    }
}
