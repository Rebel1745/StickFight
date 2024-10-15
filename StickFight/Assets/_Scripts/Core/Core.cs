using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    private readonly List<CoreComponent> _coreComponents = new List<CoreComponent>();

    private void Awake()
    {

    }

    public void LogicUpdate()
    {
        foreach (CoreComponent component in _coreComponents)
        {
            component.LogicUpdate();
        }
    }

    public void AddComponent(CoreComponent component)
    {
        if (!_coreComponents.Contains(component))
            _coreComponents.Add(component);
    }

    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var comp = _coreComponents.OfType<T>().FirstOrDefault();

        if (comp == null) Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}.");

        return comp;
    }
}
