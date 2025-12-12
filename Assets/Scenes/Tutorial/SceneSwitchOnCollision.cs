using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitchOnCollision : MonoBehaviour
{
    [Header("Scene")]
    public string sceneToLoad;

    [Header("Filter")]
    [Tooltip("Optional: Only trigger when colliding with an object with this tag.")]
    public string requiredTag = "";

    private void OnCollisionEnter(Collision collision)
    {
        // If tag filter is set, check it
        if (!string.IsNullOrEmpty(requiredTag) && !collision.gameObject.CompareTag(requiredTag))
            return;

        LoadScene();
    }

    private void LoadScene()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
            SceneManager.LoadScene(sceneToLoad);
        else
            Debug.LogError("Scene name not set on SceneSwitchOnCollision.");
    }
}
