namespace AutoMoxture.XUnit;

using System;

/// <summary>
/// Attribute to indicate that all possible values of the targeted type should be tested.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
public class ValuesAttribute : Attribute
{
}