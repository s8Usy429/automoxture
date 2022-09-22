namespace AutoMoxture.Tests.NUnit.WithSut
{
    using AutoFixture.Kernel;
    using AutoMoxture.NUnit;
    using FluentAssertions;
    using global::NUnit.Framework;

    [TestFixture]
    public class ServiceWithAmbiguousContructorTest : AutoMoxtureTest<ServiceWithAmbiguousContructor>
    {
        public ServiceWithAmbiguousContructorTest()
        {
            Customize<ServiceWithAmbiguousContructor>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
        }

        [Test]
        public void AutoMoxtureTest_ServiceWithAmbiguousContructor()
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
