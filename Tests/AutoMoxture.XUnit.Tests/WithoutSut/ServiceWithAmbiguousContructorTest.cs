namespace AutoMoxture.XUnit.Tests.WithoutSut;

using AutoFixture;
using AutoFixture.Kernel;

using AutoMoxture.Testing;

using FluentAssertions;

using Xunit;

public class ServiceWithAmbiguousContructorTest : AutoMoxtureTest
{
    public ServiceWithAmbiguousContructorTest()
    {
        this.Fixture.Customize<ServiceWithAmbiguousContructor>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }

    [Fact]
    public void AutoMoxtureTest_ServiceWithAmbiguousContructor()
    {
        // Arrange
        string prefix = this.Fixture.Create<string>();
        string demo2 = this.Fixture.Create<string>();
        this.Fixture.Mock<IDependency2>()
            .Setup(s => s.GetString())
            .Returns(demo2);
        var sut = this.Fixture.Create<ServiceWithAmbiguousContructor>();

        // Act
        var response = sut.Concat(prefix);

        // Assert
        response.Should().Contain(demo2);
    }
}
