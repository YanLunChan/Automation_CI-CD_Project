using TMPro;
using UnityEngine;

public class GameManager : SingletonClass<GameManager>
{
    [SerializeField] private TextMeshProUGUI _counterText;

    private int _counter = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    public void IncrementCounter() => _counterText.text = $"{++_counter}";

    public void ResetCounter() => _counterText.text = $"{_counter = 0}";
}
