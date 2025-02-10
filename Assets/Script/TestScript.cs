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
        Type type = obj.GetType(); // ��ü�� Ÿ�� ��������
        PropertyInfo[] properties = type.GetProperties(); // ��� �Ӽ� ��������

        foreach (PropertyInfo prop in properties)
        {
            object? value = prop.GetValue(obj); // �Ӽ� �� ��������
            Debug.Log($"{prop.Name}: {value}");
        }
    }

    private void Start()
    {
        Person person = new Person();
        PrintProperties(person);
    }
}
