using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ARscene()
    {
        SceneManager.LoadScene("AR main scene");
    }

    public void FirstScene()
    {
        SceneManager.LoadScene("firstscenee");
    }

    public void SecondScene()
    {
        SceneManager.LoadScene("secondscenee");
    }

    public void ThirdScene()
    {
        SceneManager.LoadScene("thirdscene");
    }

    public void Fourth()
    {
        SceneManager.LoadScene("my name is khan");
    }

    private IEnumerator Start()
    {
        // Check if the current scene is the starting scene
        if (SceneManager.GetActiveScene().name == "starting01")
        {
            // Wait for 3 seconds
            yield return new WaitForSeconds(2f);

            // Load the first scene after the delay
            FirstScene();
        }
    }
}
