using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveLoader : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string _sceneToLoad = "LevelTwo";

    private Animation _animation;

    private bool _isLoaded;

    private void Awake()
    {
        _animation = GetComponent<Animation>();
    }

    private void PlayDoorAnimation(AsyncOperation asyncOp = null)
    {
        if (!_isLoaded)
        {
            _animation.clip = _animation.GetClip("Open");
        }
        else
        {
            _animation.clip = _animation.GetClip("Close");
        }

        _animation.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerBehaviour>())
        {
            if (!_isLoaded)
            {
                StartCoroutine(LoadAdditive(_sceneToLoad));
            }
            else
            {
                StartCoroutine(UnloadAsync(_sceneToLoad));
            }
        }
    }

    private IEnumerator LoadAdditive(string sceneName)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        asyncOp.completed += PlayDoorAnimation;

        while (!asyncOp.isDone)
        {
            yield return null;
        }

        _isLoaded = !_isLoaded;
    }

    private IEnumerator UnloadAsync(string sceneName)
    {
        PlayDoorAnimation();

        yield return new WaitForSeconds(_animation.GetClip("Close").length);

        AsyncOperation asyncOp = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncOp.isDone)
        {
            yield return null;
        }

        _isLoaded = !_isLoaded;
    }
}
