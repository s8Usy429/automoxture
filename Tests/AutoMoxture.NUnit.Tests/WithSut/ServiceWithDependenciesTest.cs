namespace AutoMoxture.NUnit.Tests.WithSut;

using AutoFixture;

using AutoMoxture.Testing;

using FluentAssertions;

using global::NUnit.Framework;

[TestFixture]
public class ServiceWithDependenciesTest : AutoMoxtureTest<ServiceWithDependencies>
{
    [Test]
    public void AutoMoxtureTest_ServiceWithDependencies()
    {
        // Arrange
        string prefix = this.Create<string>();
        string demo2 = this.Create<string>();
        this.Mock<IDependency2>()
            .Setup(s => s.GetString())
            .Returns(demo2);

        // Act
        var response = this.Sut.Concat(prefix);

        // Assert
        response.Should().Contain(demo2);
    }
}
