namespace AutoMoxture;

using AutoFixture;
using AutoFixture.AutoMoq;

using Moq;

/// <summary>
/// Provides extension methods over AutoFixture.IFixture.
/// </summary>
public static class AutoFixtureExtensions
{
    /// <summary>
    /// Freezes the Mock of the given type.
    /// Alias method for Freeze&lt;Mock&lt;T&gt;&gt;.
    /// </summary>
    /// <param name="fixture">An instance of AutoFixture to extend.</param>
    /// <typeparam name="T">The type of the Mock.</typeparam>
    /// <returns>A frozen Mock of the given type.</returns>
    public static Mock<T> Mock<T>(this IFixture fixture) where T : class
    {
        return fixture.Freeze<Mock<T>>();
    }

    /// <summary>
    /// Enable AutoMoq customizations.
    /// </summary>
    /// <param name="fixture">An instance of AutoFixture to extend.</param>
    public static void EnableAutoMoq(this IFixture fixture)
    {
        fixture.DisableAutoMoq();
        fixture.Customize(new AutoMoqCustomization());
    }

    /// <summary>
    /// Disable AutoMoq customizations.
    /// </summary>
    /// <param name="fixture">An instance of AutoFixture to extend.</param>
    public static void DisableAutoMoq(this IFixture fixture)
    {
        for (int i = fixture.Customizations.Count - 1; i > -1; --i)
        {
            if (fixture.Customizations[i] is MockPostprocessor)
            {
                fixture.Customizations.RemoveAt(i);
            }
        }

        for (int i = fixture.ResidueCollectors.Count - 1; i > -1; --i)
        {
            if (fixture.ResidueCollectors[i] is MockRelay)
            {
                fixture.ResidueCollectors.RemoveAt(i);
            }
        }
    }
}