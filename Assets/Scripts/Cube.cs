using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Rigidbody), typeof(BoxCollider))]
[RequireComponent(typeof(ColorChanger))]
public class Cube : MonoBehaviour
{
    [SerializeField] private float _minLifeTime = 2f;
    [SerializeField] private float _maxLifeTime = 5f;

    private Renderer _renderer;
    private ColorChanger _colorChanger;
    private Color _defaultColor;
    private Coroutine _lifeCycleCoroutine = null;

    public event Action<Cube> Died;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _colorChanger = GetComponent<ColorChanger>();

        _defaultColor = _renderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Border>() == null)
            return;

        if (_lifeCycleCoroutine == null)
            _lifeCycleCoroutine = StartCoroutine(RunLifeCycle());
    }

    private void OnDisable()
    {
        if (_lifeCycleCoroutine != null)
        {
            StopCoroutine(_lifeCycleCoroutine);
            _lifeCycleCoroutine = null;
        }
    }

    private void ResetState()
    {
        _renderer.material.color = _defaultColor;
    }

    private IEnumerator RunLifeCycle()
    {
        _colorChanger.SetRandomColor(gameObject);

        yield return new WaitForSeconds(UnityEngine.Random.Range(_minLifeTime, _maxLifeTime));

        ResetState();
        Died?.Invoke(this);
    }
}
