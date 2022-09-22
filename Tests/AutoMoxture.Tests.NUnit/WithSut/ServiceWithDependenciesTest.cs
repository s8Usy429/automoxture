namespace AutoMoxture.Tests.NUnit.WithSut
{
    using AutoMoxture.NUnit;
    using FluentAssertions;
    using global::NUnit.Framework;

    [TestFixture]
    public class ServiceWithDependenciesTest : AutoMoxtureTest<ServiceWithDependencies>
    {
        [Test]
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
