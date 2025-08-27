using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class PlaymodeTests
{
    private string _testSceneName = "SampleScene";
    private Scene _currentScene;

    [UnitySetUp]
    public IEnumerator SetUp() 
    {
        yield return SceneManager.LoadSceneAsync(_testSceneName, LoadSceneMode.Additive);

        _currentScene = SceneManager.GetSceneByName(_testSceneName);
        SceneManager.SetActiveScene(_currentScene);

        yield return new WaitUntil(() => _currentScene.isLoaded);
    }
    [UnityTearDown]
    public IEnumerator TearDown() 
    {
        if(_currentScene.IsValid() && _currentScene.isLoaded)
            yield return SceneManager.UnloadSceneAsync(_currentScene);

        foreach(var obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            if(obj != null && obj.scene.name == null)
                Object.DestroyImmediate(obj);
    }

    [UnityTest]
    public IEnumerator LoadGameScene_GameManager_ShouldBeAvailable()
    {
        var instance = Object.FindFirstObjectByType<GameManager>();
        Assert.IsNotNull(instance, "Game manager wasn't found in the scene");
        
        Assert.AreEqual(_currentScene, instance.gameObject.scene, "Game manager is not in the correct scene");
        yield return null;
    }

    private Button[] CheckButtonInScene() 
    {
        Button[] buttons = GameObject.FindObjectsByType<Button>(FindObjectsSortMode.InstanceID);

        if(buttons.Length == 0)
            Assert.Fail($"Currently there's no button in {_currentScene.name}");

        return buttons;
    }

    private void IncrementButtonPressed(Button[] buttons, out Button reset, int increment) 
    {
        reset = null;
        foreach (var button in buttons)
        {
            if (button.gameObject.name == "Increment")
                for (int i = 0; i < increment; i++)
                    button.onClick.Invoke();
            else if (button.gameObject.name == "Reset")
                reset = button;
        }
    }

    [UnityTest]
    public IEnumerator Counter_Increment_WhenButtonPressed() 
    {
        Button[] buttons = CheckButtonInScene();

        IncrementButtonPressed(buttons, out _ ,1);
        
        yield return null;

        Assert.AreEqual(GameManager.Instance.Counter, 1, $"Button does not work as intended. Expected result is that counter should be one, Count is {GameManager.Instance.Counter}");
    }

    [UnityTest]
    public IEnumerator Counter_Reset_WhenButtonPressed()
    {
        Button[] buttons = CheckButtonInScene();

        IncrementButtonPressed(buttons, out Button reset, 2);

        yield return null;

        Assert.NotNull(reset, $"{_currentScene} does not contain a reset button");
        reset.onClick.Invoke();

        Assert.AreEqual(GameManager.Instance.Counter, 0, $"Button does not work as intended. Expected result is that counter should be 0, Count is {GameManager.Instance.Counter}");
    }
}
