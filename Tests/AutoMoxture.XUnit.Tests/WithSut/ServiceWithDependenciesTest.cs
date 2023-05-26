namespace AutoMoxture.XUnit.Tests.WithSut;

using AutoFixture;

using AutoMoxture.Testing;

using FluentAssertions;

using Xunit;

public class ServiceWithDependenciesTest : AutoMoxtureTest<ServiceWithDependencies>
{
    [Fact]
    public void AutoMoxtureTest_ServiceWithDependencies()
    {
        // Arrange
        string prefix = this.Fixture.Create<string>();
        string demo2 = this.Fixture.Create<string>();
        this.Fixture.Mock<IDependency2>()
            .Setup(s => s.GetString())
            .Returns(demo2);

        // Act
        var response = this.Sut.Concat(prefix);

        // Assert
        response.Should().Contain(demo2);
    }
}
