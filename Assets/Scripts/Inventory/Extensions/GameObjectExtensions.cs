using UnityEngine;
using System;

public static class GameObjectExtensions
{
    /// <summary>
    /// Creates a new child GameObject and adds it to the parent with specified components.
    /// </summary>
    public static GameObject CreateChild(this GameObject parent,string name, params Type[] componentTypes)
    {
        var child = new GameObject("Child");
        child.name = name;  
        child.transform.SetParent(parent.transform, false);
        child.AddComponents(componentTypes);
        return child;
    }
    public static GameObject CreateBoxView(this GameObject parent, string name)
    {
        return CreateChild(parent, name, typeof(SpriteRenderer), typeof(BoxCollider2D));
    }
    /// <summary>
    /// Creates a new child GameObject with a specific MonoBehaviour component and adds additional components.
    /// </summary>
    public static T CreateChild<T>(this GameObject parent, string name, params Type[] componentTypes) where T : MonoBehaviour
    {
        var child = new GameObject(typeof(T).Name);
        child.name = name;
        child.AddComponent<T>();
        child.transform.SetParent(parent.transform, false);
        child.AddComponents(componentTypes);
        return child.GetComponent<T>();
    }

    /// <summary>
    /// Adds a GameObject as a child of the parent GameObject.
    /// </summary>
    //public static T AddTo<T>(this T child, GameObject parent) where T : Component
    //{
    //    child.transform.SetParent(parent.transform, false);
    //    return child;
    //}

    /// <summary>
    /// Adds one or more components to the GameObject.
    /// </summary>
    public static GameObject AddComponents(this GameObject gameObject, params Type[] componentTypes)
    {
        foreach (Type componentType in componentTypes)
        {
            if (componentType != null && typeof(Component).IsAssignableFrom(componentType))
            {
                gameObject.AddComponent(componentType);
            }
            else
            {
                Debug.LogWarning($"Type '{componentType?.Name}' is not a valid Component type.");
            }
        }
        return gameObject;
    }

    /// <summary>
    /// Adds a specific MonoBehaviour component to the GameObject (similar to adding a manipulator).
    /// </summary>
    //public static GameObject WithComponent<T>(this GameObject gameObject) where T : MonoBehaviour
    //{
    //    gameObject.AddComponent<T>();
    //    return gameObject;
    //}
    public static T OrNull<T>(this T obj) where T : UnityEngine.Object => obj ? obj : null;
}