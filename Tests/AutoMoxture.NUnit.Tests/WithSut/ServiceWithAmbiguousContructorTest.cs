namespace AutoMoxture.NUnit.Tests.WithSut;

using AutoFixture;
using AutoFixture.Kernel;

using AutoMoxture.Testing;

using FluentAssertions;

using global::NUnit.Framework;

[TestFixture]
public class ServiceWithAmbiguousContructorTest : AutoMoxtureTest<ServiceWithAmbiguousContructor>
{
    public ServiceWithAmbiguousContructorTest()
    {
        this.Fixture.Customize<ServiceWithAmbiguousContructor>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }

    [Test]
    public void AutoMoxtureTest_ServiceWithAmbiguousContructor()
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
