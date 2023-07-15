using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTags : MonoBehaviour
{
    [SerializeField] public List<string> customTagsList = new List<string>();

    // Example usage: Add a tag to the TagHolder
    public void AddTag(string customTag)
    {
        if (!customTagsList.Contains(customTag))
        {
            customTagsList.Add(customTag);
        }
    }

    // Example usage: Remove a tag from the TagHolder
    public void RemoveTag(string customTag)
    {
        if (customTagsList.Contains(customTag))
        {
            customTagsList.Remove(customTag);
        }
    }

    // Example usage: Check if the TagHolder has a specific tag
    public bool HasTag(string customTag)
    {
        return customTagsList.Contains(customTag);
    }
}
