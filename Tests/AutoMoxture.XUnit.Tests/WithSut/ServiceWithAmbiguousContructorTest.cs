namespace AutoMoxture.XUnit.Tests.WithSut;

using AutoFixture;
using AutoFixture.Kernel;

using AutoMoxture.Testing;

using FluentAssertions;

using Xunit;

public class ServiceWithAmbiguousContructorTest : AutoMoxtureTest<ServiceWithAmbiguousContructor>
{
    public ServiceWithAmbiguousContructorTest()
    {
        this.Customize<ServiceWithAmbiguousContructor>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }

    [Fact]
    public void AutoMoxtureTest_ServiceWithAmbiguousContructor()
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
