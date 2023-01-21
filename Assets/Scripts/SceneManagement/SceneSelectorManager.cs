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

    public void Stage01( ) {
        SceneManager.LoadSceneAsync( "Stage01" );
    }
    public void Stage02( ) {
        SceneManager.LoadSceneAsync("Stage02");
    }
    public void Gacha()
    {
        SceneManager.LoadSceneAsync("GachaScene");
    }
}
