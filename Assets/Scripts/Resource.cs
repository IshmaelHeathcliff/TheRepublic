using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class Resource
{
    [TabGroup("Produced")][TableList][SerializeField] List<ResourceData> resourcesProduced;
    [TabGroup("Possessed")][TableList][SerializeField] List<ResourceData> resourcesPossessed;
    [TabGroup("Consumed")][TableList][SerializeField] List<ResourceData> resourcesConsumed;

    readonly Dictionary<ResourceState, List<ResourceData>> _resourceMap;
    public Resource()
    {
        resourcesProduced = new List<ResourceData>();
        resourcesPossessed = new List<ResourceData>();
        resourcesConsumed = new List<ResourceData>();
        foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
        {
            resourcesProduced.Add(new ResourceData(resourceType));
            resourcesPossessed.Add(new ResourceData(resourceType));
            resourcesConsumed.Add(new ResourceData(resourceType));
        }

        _resourceMap = new Dictionary<ResourceState, List<ResourceData>>
        {
            {ResourceState.Produced, resourcesProduced},
            {ResourceState.Possessed, resourcesPossessed},
            {ResourceState.Consumed, resourcesConsumed}
        };
    }

    public float this[ResourceState state, ResourceType type]
    {
        get
        {
            foreach (ResourceData data in _resourceMap[state].Where(data => data.type == type))
            {
                return data.volume;
            }

            Debug.Log("无效资源类型");
            return 0;
        }

        set
        {

            foreach (ResourceData data in _resourceMap[state].Where(data => data.type == type))
            {
                data.volume = value;
            }
        }
    }

}

[Serializable]
public class ResourceData
{
    public ResourceType type;
    public float volume;
    
    public ResourceData(ResourceType type)
    {
        this.type = type;
        volume = 0;
    }
}

public enum ResourceType
{
    Food,
    Education,
    Entertainment,
    Housing,
}

public enum ResourceState
{
    Produced,
    Possessed,
    Consumed
}