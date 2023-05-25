namespace AutoMoxture.XUnit.Tests.WithSut
{
    using AutoFixture;
    using AutoFixture.Kernel;
    using AutoMoxture.Testing;
    using AutoMoxture.XUnit;
    using FluentAssertions;
    using Xunit;

    public class ServiceWithAmbiguousContructorTest : AutoMoxtureTest<ServiceWithAmbiguousContructor>
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

            // Act
            var response = this.Sut.Concat(prefix);

            // Assert
            response.Should().Contain(demo2);
        }
    }
}
