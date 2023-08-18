using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
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
        public void Test(PeopleDataType peopleDataType, ResourceType resourceType, ResourceState resourceState,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null, bool reset = false, int num = 1)
        {
                if (reset)
                {
                        UpdatePopulation(0, false, occupation, age, gender);
                        UpdateData(0, peopleDataType, false, occupation, age, gender);
                        UpdateResource(0, resourceState, resourceType, false, occupation, age, gender);
                        return;
                }
                
                Debug.Log($"Number before: {GetPopulation(occupation, age, gender)}");
                UpdatePopulation(num, true, occupation, age, gender);
                Debug.Log($"Number after: {GetPopulation(occupation, age, gender)}");
                
                Debug.Log($"Data before: {GetData(peopleDataType, occupation, age, gender)}");
                UpdateData(num, peopleDataType, true, occupation, age, gender);
                Debug.Log($"Data after: {GetData(peopleDataType, occupation, age, gender)}");
                
                Debug.Log($"Resource before: {GetResource(resourceState, resourceType, occupation, age, gender)}");
                UpdateResource(num, resourceState, resourceType, true, occupation, age, gender);
                Debug.Log($"Resource after: {GetResource(resourceState, resourceType, occupation, age, gender)}");
        }

        IEnumerable<PeopleData> GetPeopleOfType(
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                return data.Where(datum =>
                        (occupation == PeopleType.Occupation.Null || datum.type.occupation == occupation) &&
                        (age == PeopleType.AgeType.Null || datum.type.age == age) &&
                        (gender == PeopleType.Gender.Null || datum.type.gender == gender));
        }

        public int GetPopulation(PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                                 PeopleType.AgeType age = PeopleType.AgeType.Null,
                                 PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                return GetPeopleOfType(occupation, age, gender).Sum(datum => datum.number);
        }
        
        void UpdatePopulation(int number, bool changeMode = false,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null, 
                PeopleType.AgeType age = PeopleType.AgeType.Null, 
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                foreach (PeopleData datum in GetPeopleOfType(occupation, age, gender))
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

        public void SetPopulation(int number,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                UpdatePopulation(number, false, occupation, age, gender);
        }

        public void ChangePopulation(int number,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                UpdatePopulation(number, true, occupation, age, gender);
        }
        
        

        public float GetData(PeopleDataType peopleDataType, 
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                return GetPeopleOfType(occupation, age, gender).Sum(datum => datum[peopleDataType]);
        }
        
        void UpdateData(float value, PeopleDataType peopleDataType, bool changeMode = false,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                foreach (PeopleData datum in GetPeopleOfType(occupation, age, gender))
                {
                        if (changeMode)
                        {
                                datum[peopleDataType] += value;
                        }
                        else
                        {
                                datum[peopleDataType] = value;
                        }
                }
        }

        public void SetData(float value, PeopleDataType peopleDataType,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                UpdateData(value, peopleDataType, false, occupation, age, gender);
        }
        
        public void ChangeData(float value, PeopleDataType peopleDataType,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                UpdateData(value, peopleDataType, true, occupation, age, gender);
        }

        public float GetResource(ResourceState resourceState, ResourceType resourceType, 
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                return GetPeopleOfType(occupation, age, gender).Sum(datum => datum[resourceState, resourceType]);
        }
        
        void UpdateResource(float value, ResourceState resourceState, ResourceType resourceType, bool changeMode = false,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                foreach (PeopleData datum in GetPeopleOfType(occupation, age, gender))
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

        public void SetResource(float value, ResourceState resourceState, ResourceType resourceType,
                bool changeMode = false,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                UpdateResource(value, resourceState, resourceType, false, occupation, age, gender);
        }
        
        public void ChangeResource(float value, ResourceState resourceState, ResourceType resourceType,
                bool changeMode = false,
                PeopleType.Occupation occupation = PeopleType.Occupation.Null,
                PeopleType.AgeType age = PeopleType.AgeType.Null,
                PeopleType.Gender gender = PeopleType.Gender.Null)
        {
                UpdateResource(value, resourceState, resourceType, true, occupation, age, gender);
        }
}