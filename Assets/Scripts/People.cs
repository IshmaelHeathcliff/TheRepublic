using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu(fileName = "People", menuName = "ScriptableObjects/People")]
public class People : SerializedScriptableObject
{
        public Dictionary<PeopleType, PeopleData> data;
        public People()
        {
                data = new Dictionary<PeopleType, PeopleData>();
                foreach (PeopleType.Occupation type in Enum.GetValues(typeof(PeopleType.Occupation)))
                {
                        foreach (PeopleType.AgeType age in Enum.GetValues(typeof(PeopleType.AgeType)))
                        {
                                foreach (PeopleType.Gender gender in Enum.GetValues(typeof(PeopleType.Gender)))
                                {
                                        data.Add(
                                                new PeopleType
                                                {
                                                        type = type,
                                                        age = age,
                                                        gender = gender
                                                },
                                                new PeopleData());
                                }
                        }
                }
        }
}

public class PeopleType
{
        public enum Occupation
        {
                Resident,
                Peasant,
                Soldier,
                Politician,
                Scientist,
                Artist,
                Merchant
        }

        public enum AgeType
        {
                Young,
                Middle,
                Old
        }

        public enum Gender
        {
                Men,
                Women
        }

        [BoxGroup("类别")] public Occupation type;
        [BoxGroup("类别")] public AgeType age;
        [BoxGroup("类别")] public Gender gender;
}

public class PeopleData
{
        public int number;
        [BoxGroup("数值")] public float productivity;
        [BoxGroup("数值")]public float consuming;
        [BoxGroup("数值")]public float strength;
        [BoxGroup("数值")]public float intelligence;
        [BoxGroup("数值")]public float culture;
        [BoxGroup("数值")]public float loyalty;
        [BoxGroup("数值")]public float morality;
        [BoxGroup("数值")]public float happiness;
        [BoxGroup("数值")]public float reproduce;
        [BoxGroup("数值")]public float sociability;
        
        [BoxGroup("资源")] public Dictionary<ResourceType, float> resourcesProduced;
        [BoxGroup("资源")] public Dictionary<ResourceType, float> resourcesPossessed;
        [BoxGroup("资源")] public Dictionary<ResourceType, float> resourcesConsumed;

        public PeopleData()
        {
                resourcesProduced = new Dictionary<ResourceType, float>();
                resourcesPossessed = new Dictionary<ResourceType, float>();
                resourcesConsumed = new Dictionary<ResourceType, float>();
                foreach (ResourceType resourceType in Enum.GetValues(typeof(ResourceType)))
                {
                        resourcesProduced.Add(resourceType, 0);
                        resourcesPossessed.Add(resourceType, 0);
                        resourcesConsumed.Add(resourceType, 0);
                }
        }
}

public enum ResourceType
{
        Food,
        Education,
        Entertainment,
        Housing,
}