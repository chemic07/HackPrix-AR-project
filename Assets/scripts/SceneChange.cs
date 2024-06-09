using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
  
    public void Startscene()
    {
        SceneManager.LoadScene("starting scene");
    }
    public void secondscene()
    {
        SceneManager.LoadScene("Second scene");
    }
    public void Thirdscene()
    {
        SceneManager.LoadScene("Third Scene");
    }
    
    public void ARview()
    {
        SceneManager.LoadScene("AR View");
    }

    public void walkthrough()
    {
        SceneManager.LoadScene("walkthrougg");
    }
}
