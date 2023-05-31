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
        
        [Button]
        public void Test(PeopleData.DataType dataType, ResourceType resourceType, ResourceState resourceState,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null, bool reset = false, int num = 1)
        {
                if (reset)
                {
                        SetPopulation(0, false, occupation, age, gender);
                        SetData(0, dataType, false, occupation, age, gender);
                        SetResource(0, resourceState, resourceType, false, occupation, age, gender);
                        return;
                }
                
                Debug.Log($"Number before: {GetPopulation(occupation, age, gender)}");
                SetPopulation(num, true, occupation, age, gender);
                Debug.Log($"Number after: {GetPopulation(occupation, age, gender)}");
                
                Debug.Log($"Data before: {GetData(dataType, occupation, age, gender)}");
                SetData(num, dataType, true, occupation, age, gender);
                Debug.Log($"Data after: {GetData(dataType, occupation, age, gender)}");
                
                Debug.Log($"Resource before: {GetResource(resourceState, resourceType, occupation, age, gender)}");
                SetResource(num, resourceState, resourceType, true, occupation, age, gender);
                Debug.Log($"Resource after: {GetResource(resourceState, resourceType, occupation, age, gender)}");
        }

        public int GetPopulation(PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                                 PeopleType.AgeType age = PeopleType.AgeType.Null,
                                 PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                var sum = 0;
                foreach (PeopleData datum in data)
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
        
        public void SetPopulation(int number, bool changeMode = false,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null, 
                PeopleType.AgeType age = PeopleType.AgeType.Null, 
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                foreach (PeopleData datum in data)
                {
                        if ((occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                            (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                            (gender == PeopleType.Gender.Null || datum.type.gender == gender))
                        {
                                if (changeMode)
                                {
                                        datum.number += number;
                                }
                                else
                                {
                                        datum.number = number;
                                }
                        }
                        
                }
                
        }

        public float GetData(PeopleData.DataType dataType, 
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
                                result += datum[dataType];
                        }
                }

                return result;

        }
        
        public void SetData(float value, PeopleData.DataType dataType, bool changeMode = false,
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
                                if (changeMode)
                                {
                                        datum[dataType] += value;
                                }
                                else
                                {
                                        datum[dataType] = value;
                                }
                                
                        }
                }
        }

        public float GetResource(ResourceState resourceState, ResourceType resourceType, 
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
                                result += datum[resourceState, resourceType];
                        }
                }

                return result;
                
        }
        
        public void SetResource(float value, ResourceState resourceState, ResourceType resourceType, bool changeMode = false,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                foreach (PeopleData datum in data)
                {
                        if ((occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                            (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                            (gender == PeopleType.Gender.Null || datum.type.gender == gender))
                        {
                                if (changeMode)
                                {
                                        datum[resourceState, resourceType] += value;
                                }
                                else
                                {
                                        datum[resourceState, resourceType] = value;
                                }
                                
                                
                        }
                }

                
        }
}