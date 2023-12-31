using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class PeopleData
{
        [TableColumnWidth(160, Resizable = false), LabelWidth(80)]
        public PeopleType type;
        
        [TableColumnWidth(60, Resizable = false)]
        public int number;
        
        [SerializeReference][TableColumnWidth(250, Resizable = false)]
        public Dictionary<PeopleDataType, float> data;

        [TableColumnWidth(300, Resizable = false)]
        public Resource resource;

        public PeopleData(PeopleType type)
        {
                this.type = type;
                resource = new Resource();

                data = new Dictionary<PeopleDataType, float>();
                foreach (PeopleDataType dataType in Enum.GetValues(typeof(PeopleDataType)))
                {
                        data.Add(dataType, 0f);
                }
        }

        // [Button]
        // public void Test()
        // {
        //         var dataType = DataType.Productivity;
        //         Debug.Log($"Data before: {this[dataType]}");
        //         this[dataType] += 1;
        //         Debug.Log($"Data after: {this[dataType]}");
        //         
        //         var resourceState = ResourceState.Possessed;
        //         var resourceType = ResourceType.Food;
        //         Debug.Log($"Resource before: {this[resourceState, resourceType]}");
        //         this[resourceState, resourceType] += 1;
        //         Debug.Log($"Resource after: {this[resourceState, resourceType]}");
        // }

        public float this[PeopleDataType peopleDataType]
        {
                get => data[peopleDataType];
                set => data[peopleDataType] = value;
        }


        public float this[ResourceState resourceState, ResourceType resourceType]
        {
                get => resource[resourceState, resourceType];
                set => resource[resourceState, resourceType] = value;
        }
}

public enum PeopleDataType
{
        Productivity,
        Consuming,
        Strength,
        Intelligence,
        Culture,
        Loyalty,
        Morality,
        Happiness,
        Reproduce,
        Sociability,
}

[Serializable]
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
                Merchant,
                Null
        }

        public enum AgeType
        {
                Young,
                Middle,
                Old,
                Null
        }

        public enum Gender
        {
                Men,
                Women,
                Null
        }

        [BoxGroup("类别")] public Occupation occupation;
        [BoxGroup("类别")] public AgeType age;
        [BoxGroup("类别")] public Gender gender;

        public PeopleType(Occupation occupation, AgeType age, Gender gender)
        {
                this.occupation = occupation;
                this.age = age;
                this.gender = gender;
        }
}