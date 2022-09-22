namespace AutoMoxture.Tests.XUnit.WithoutSut
{
    using AutoMoxture.XUnit;
    using FluentAssertions;
    using Xunit;

    public class ServiceWithDependenciesTest : AutoMoxtureTest
    {
        [Fact]
        public void AutoMoxtureTest_ServiceWithDependencies()
        {
            // Arrange
            string prefix = Create<string>();
            string demo2 = Create<string>();
            Mock<IDependency2>()
                .Setup(s => s.GetString())
                .Returns(demo2);
            var sut = Create<ServiceWithDependencies>();

            // Act
            var response = sut.Concat(prefix);

            // Assert
            response.Should().Contain(demo2);
        }
    }
}
