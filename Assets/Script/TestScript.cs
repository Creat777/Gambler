using UnityEngine;
using System;
using System.Reflection;


public class TestScript : MonoBehaviour
{
    class Person
    {
        public string Name { get; set; } = "Alice";
        public int Age { get; set; } = 25;
        public bool IsActive { get; set; } = true;
    }

    static void PrintProperties(object obj)
    {
        Type type = obj.GetType(); // 객체의 타입 가져오기
        PropertyInfo[] properties = type.GetProperties(); // 모든 속성 가져오기

        foreach (PropertyInfo prop in properties)
        {
            object? value = prop.GetValue(obj); // 속성 값 가져오기
            Debug.Log($"{prop.Name}: {value}");
        }
    }

    private void Start()
    {
        Person person = new Person();
        PrintProperties(person);
    }
}
