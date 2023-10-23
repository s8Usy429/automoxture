namespace AutoMoxture.XUnit;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

/// <summary>
/// Attribute to have the test auto-generate its input values.
/// The attribute takes into account the usage of [Values] attributes to generate all possible values for a type.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class AutoArrangeAttribute : InlineAutoDataAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoArrangeAttribute"/> class.
    /// </summary>
    /// <param name="values">The data values to pass to the theory.</param>
    public AutoArrangeAttribute(params object[] values)
        : base(new AutoMoqDataAttribute(), values)
    {
    }

    /// <inheritdoc />
    public override IEnumerable<object[]> GetData(MethodInfo testMethod)
    {
        // Anticipating callers will mistakenly use both empty [AutoArrange] and [AutoArrange("something")] => Ignore empty ones
        if (!this.Values.Any() && testMethod.GetCustomAttributes<AutoArrangeAttribute>().Any(att => att.Values.Any()))
        {
            return Enumerable.Empty<object[]>();
        }

        var arraysToCombine = base.GetData(testMethod).ToArray()[0]
            .Select(x => new object[] { x })
            .ToArray();

        var parameters = testMethod.GetParameters();
        for (int i = 0; i < parameters.Length; ++i)
        {
            if (parameters[i].GetCustomAttribute<ValuesAttribute>() is not null)
            {
                if (parameters[i].ParameterType.IsEnum)
                {
                    arraysToCombine[i] = Enum
                        .GetValues(parameters[i].ParameterType)
                        .Cast<object>()
                        .Distinct()
                        .ToArray();
                }
                else if (parameters[i].ParameterType == typeof(bool))
                {
                    arraysToCombine[i] = new object[]
                    {
                        false,
                        true,
                    };
                }
            }
        }

        return GenerateAllPossibleCombinations(arraysToCombine);
    }

    private static IEnumerable<object[]> GenerateAllPossibleCombinations(IReadOnlyCollection<IReadOnlyCollection<object>> bucketsToCombine)
    {
        IEnumerable<object[]> allCombinations = new List<object[]>() { Array.Empty<object>() };

        foreach (var bucketToCombine in bucketsToCombine)
        {
            allCombinations = allCombinations
                .SelectMany(combination => bucketToCombine
                    .Select(x => new List<object>(combination) { x }.ToArray())
                    .ToList());
        }

        return allCombinations;
    }

    private class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute()
            : base(() => new Fixture().Customize(new AutoMoqCustomization()))
        {
        }
    }
}
