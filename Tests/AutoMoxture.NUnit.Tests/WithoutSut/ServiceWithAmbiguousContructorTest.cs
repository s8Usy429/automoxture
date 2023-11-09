namespace AutoMoxture.NUnit.Tests.WithoutSut;

using AutoFixture;
using AutoFixture.Kernel;

using AutoMoxture.Testing;

using FluentAssertions;

using global::NUnit.Framework;

public class ServiceWithAmbiguousContructorTest : AutoMoxtureTest
{
    public ServiceWithAmbiguousContructorTest()
    {
        this.Customize<ServiceWithAmbiguousContructor>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }

    [Test]
    public void AutoMoxtureTest_ServiceWithAmbiguousContructor()
    {
        // Arrange
        string prefix = this.Create<string>();
        string demo2 = this.Create<string>();
        this.Mock<IDependency2>()
            .Setup(s => s.GetString())
            .Returns(demo2);
        var sut = this.Create<ServiceWithAmbiguousContructor>();

        // Act
        var response = sut.Concat(prefix);

        // Assert
        response.Should().Contain(demo2);
    }
}
