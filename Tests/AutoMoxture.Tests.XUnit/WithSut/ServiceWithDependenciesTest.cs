namespace AutoMoxture.Tests.XUnit.WithSut
{
    using AutoMoxture.XUnit;
    using FluentAssertions;
    using Xunit;

    public class ServiceWithDependenciesTest : AutoMoxtureTest<ServiceWithDependencies>
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

            // Act
            var response = Sut.Concat(prefix);

            // Assert
            response.Should().Contain(demo2);
        }
    }
}
