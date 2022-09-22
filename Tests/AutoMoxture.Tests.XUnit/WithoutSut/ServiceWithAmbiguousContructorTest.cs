namespace AutoMoxture.Tests.XUnit.WithoutSut
{
    using AutoFixture.Kernel;
    using AutoMoxture.XUnit;
    using FluentAssertions;
    using Xunit;

    public class ServiceWithAmbiguousContructorTest : AutoMoxtureTest
    {
        public ServiceWithAmbiguousContructorTest()
        {
            Customize<ServiceWithAmbiguousContructor>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
        }

        [Fact]
        public void AutoMoxtureTest_ServiceWithAmbiguousContructor()
        {
            // Arrange
            string prefix = Create<string>();
            string demo2 = Create<string>();
            Mock<IDependency2>()
                .Setup(s => s.GetString())
                .Returns(demo2);
            var sut = Create<ServiceWithAmbiguousContructor>();

            // Act
            var response = sut.Concat(prefix);

            // Assert
            response.Should().Contain(demo2);
        }
    }
}
