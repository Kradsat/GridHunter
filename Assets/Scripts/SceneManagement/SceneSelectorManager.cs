using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSelectorManager : MonoBehaviour
{
    public void Title( ) {
        SceneManager.LoadSceneAsync( "TitleScene" );
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayBGAudio();
    }
    public void StageSelector(  ) {
        SceneManager.LoadSceneAsync( "StageSelectorScene" );
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().PlayBGAudio();
    }

    public void Stage01( ) {
        SceneManager.LoadSceneAsync( "Stage01" );
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopBGAudio();
    }
    public void Stage02( ) {
        SceneManager.LoadSceneAsync("scene2");
        GameObject.FindGameObjectWithTag("Music").GetComponent<MusicClass>().StopBGAudio();
    }
    public void Gacha()
    {
        SceneManager.LoadSceneAsync("GachaScene");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
