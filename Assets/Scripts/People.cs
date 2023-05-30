using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "People", menuName = "ScriptableObjects/People")]
public class People : SerializedScriptableObject
{
        [TableList(ShowIndexLabels = true)]
        public List<PeopleData> data;
        public People()
        {
                data = new List<PeopleData>();
                foreach (PeopleType.Occupation occupation in Enum.GetValues(typeof(PeopleType.Occupation)))
                {
                        if (occupation == PeopleType.Occupation.Null) continue;
                        
                        foreach (PeopleType.AgeType age in Enum.GetValues(typeof(PeopleType.AgeType)))
                        {
                                if (age == PeopleType.AgeType.Null) continue;
                                
                                foreach (PeopleType.Gender gender in Enum.GetValues(typeof(PeopleType.Gender)))
                                {
                                        if (gender == PeopleType.Gender.Null) continue;
                                        
                                        data.Add(new PeopleData(new PeopleType(occupation, age, gender)));
                                }
                        }
                }
        }

        public bool HasPeopleType(PeopleType.Occupation occupation, PeopleType.AgeType age, PeopleType.Gender gender)
        {
                foreach (var datum in data)
                {
                        if (datum.type.occupation == occupation && datum.type.age == age && datum.type.gender == gender)
                                return true;
                }

                return false;
        }

        public void AddPeople(PeopleType.Occupation occupation, PeopleType.AgeType age, PeopleType.Gender gender)
        {
                if(!HasPeopleType(occupation, age, gender))
                        data.Add(new PeopleData(new PeopleType(occupation, age, gender)));
        }

        public int GetPopulation(PeopleType.Occupation occupation = PeopleType.Occupation.Null, 
                                 PeopleType.AgeType age = PeopleType.AgeType.Null, 
                                 PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                var sum = 0;
                foreach (var datum in data)
                {
                        if ((occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                            (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                            (gender == PeopleType.Gender.Null || datum.type.gender == gender))
                        {
                                sum += datum.number;
                        }
                        
                }

                return sum;
        }
        
        public void SetPopulation(int number, int bias = 0,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null, 
                PeopleType.AgeType age = PeopleType.AgeType.Null, 
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                foreach (var datum in data)
                {
                        if ((occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                            (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                            (gender == PeopleType.Gender.Null || datum.type.gender == gender))
                        {
                                datum.number = number + bias;
                        }
                        
                }
                
        }

        public float GetData(DataType dataType, 
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                float result = 0f;
                foreach (var datum in data)
                {
                        if ((occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                            (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                            (gender == PeopleType.Gender.Null || datum.type.gender == gender))
                        {
                                result += datum.GetData(dataType);
                        }
                }

                return result;

        }
        
        public void SetData(float value, DataType dataType, float bias = 0,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                foreach (var datum in data)
                {
                        if ((occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                            (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                            (gender == PeopleType.Gender.Null || datum.type.gender == gender))
                        {
                                datum.SetData(value, dataType, bias);
                        }
                }
        }

        public float GetResource(ResourceType resourceType, ResourceState resourceState,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                float result = 0f;
                foreach (var datum in data)
                {
                        if ((occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                            (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                            (gender == PeopleType.Gender.Null || datum.type.gender == gender))
                        {
                                result += datum.GetResource(resourceState, resourceType);
                        }
                }

                return result;
                
        }
        
        public void SetResource(float value, ResourceType resourceType, ResourceState resourceState, float bias = 0,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                foreach (var datum in data)
                {
                        if ((occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                            (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                            (gender == PeopleType.Gender.Null || datum.type.gender == gender))
                        {
                                datum.SetResource(value, resourceState, resourceType, bias);
                        }
                }

                
        }
}


[Serializable]
public class PeopleData
{
        [TableColumnWidth(160, Resizable = false), LabelWidth(80)]
        public PeopleType type;
        
        [TableColumnWidth(60, Resizable = false)]
        public int number;
        
        [VerticalGroup("Group 1"), LabelWidth(80), TableColumnWidth(150, Resizable = false)]
        public float productivity, consuming, reproduce, sociability;
        [VerticalGroup("Group 2"), LabelWidth(80), TableColumnWidth(150, Resizable = false)]
        public float strength, intelligence;

        [VerticalGroup("Group 3"), LabelWidth(80), TableColumnWidth(150, Resizable = false)] 
        public float culture, loyalty, morality, happiness;

        [TableColumnWidth(300)]
        public Resource resource;

        public PeopleData(PeopleType type)
        {
                this.type = type;
                resource = new Resource();
        }

        public float GetData(DataType dataType)
        {
                return dataType switch
                {
                        DataType.Productivity => productivity,
                        DataType.Consuming => consuming,
                        DataType.Strength => strength,
                        DataType.Intelligence => intelligence,
                        DataType.Culture => culture,
                        DataType.Loyalty => loyalty,
                        DataType.Morality => morality,
                        DataType.Happiness => happiness,
                        DataType.Reproduce => reproduce,
                        DataType.Sociability => sociability,
                        _ => throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null)
                };
        }

        public void SetData(float value, DataType dataType, float bias = 0f)
        {
                switch (dataType)
                {
                        case DataType.Productivity:
                                productivity = value + bias;
                                break; 
                        case DataType.Consuming:
                                consuming = value + bias;
                                break;
                        case DataType.Strength:
                                strength = value + bias;
                                break;
                        case DataType.Intelligence:
                                intelligence = value + bias;
                                break;
                        case DataType.Culture:
                                culture = value + bias;
                                break;
                        case DataType.Loyalty:
                                loyalty = value + bias;
                                break;
                        case DataType.Morality:
                                morality = value + bias;
                                break;
                        case DataType.Happiness:
                                happiness = value + bias;
                                break;
                        case DataType.Reproduce:
                                reproduce = value + bias;
                                break;
                        case DataType.Sociability:
                                sociability = value + bias;
                                break;
                        default:
                                throw new ArgumentOutOfRangeException(nameof(dataType), dataType, null);
                }
        }

        public float GetResource(ResourceState resourceState, ResourceType resourceType)
        {
                return resource[resourceState, resourceType];
        }
        
        public void SetResource(float value, ResourceState resourceState, ResourceType resourceType, float bias = 0f)
        {
                resource[resourceState, resourceType] = value + bias;
        }
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

public enum DataType
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