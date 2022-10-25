using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectorManager : MonoBehaviour
{
    public void Title( ) {
        SceneManager.LoadSceneAsync( "TitleScene" );
    }
    public void StageSelector(  ) {
        SceneManager.LoadSceneAsync( "StageSelectorScene" );    
    }

    public void Stage1( ) {
        SceneManager.LoadSceneAsync( "GameScene" );
    }
}
