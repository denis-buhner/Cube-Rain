using UnityEngine;

[RequireComponent(typeof (Renderer))]
public class ColorChanger : MonoBehaviour
{
    public void SetRandomColor(GameObject gameObject)
    {
        Renderer renderer = gameObject.GetComponent<Renderer>();
        renderer.material.color = Random.ColorHSV();
    }
}