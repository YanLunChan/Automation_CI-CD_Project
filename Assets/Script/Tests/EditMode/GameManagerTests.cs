using NUnit.Framework;
using TMPro;
using UnityEngine;
public class GameManagerTests
{
    [SetUp]
    public void Setup()
    {
        var go = new GameObject("GameManager");
        GameManager instance = go.AddComponent<GameManager>();

        // Create a mock text object for testing
        var textGo = new GameObject("CounterText");
        var tmpText = textGo.AddComponent<TextMeshProUGUI>();
        instance.SetCounterText(tmpText);

        instance.SetInstance();
    }
    [TearDown]
    public void TearDown() 
    {
        if (GameManager.Instance != null)
            Object.DestroyImmediate(GameManager.Instance.gameObject);
    }

    [Test]
    public void Counter_ShouldIncrement()
    {
        GameManager instance = GameManager.Instance;

        instance.IncrementCounter();

        Assert.AreEqual(1, instance.Counter, $"Counter is at: {instance.Counter}. Expected result should be 1");
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public void Counter_ShouldIncrement_AfterMultipleUse(int iteration)
    {
        GameManager instance = GameManager.Instance;

        for (int i = 0; i < iteration; i++)
            instance.IncrementCounter();

        Assert.AreEqual(iteration, instance.Counter, $"Counter is at: {instance.Counter}. Expected result should be {iteration}");
    }

    [TestCase(0)]
    [TestCase(1)]
    [TestCase(2)]
    [TestCase(5)]
    public void Counter_ShouldResetToZero_WhenCalled_AfterMultipleUse(int iteration)
    {
        GameManager instance = GameManager.Instance;

        for (int i = 0; i < iteration; i++)
            instance.IncrementCounter();
        
        instance.ResetCounter();

        Assert.AreEqual(0, instance.Counter, $"Counter is at: {instance.Counter}. Expected result should be {0}");
    }
}
