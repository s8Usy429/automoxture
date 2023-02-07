namespace AutoMoxture
{
    using AutoFixture;
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
        public static Mock<T> Mock<T>(this IFixture fixture) where T : class
        {
            return fixture.Freeze<Mock<T>>();
        }
    }
}