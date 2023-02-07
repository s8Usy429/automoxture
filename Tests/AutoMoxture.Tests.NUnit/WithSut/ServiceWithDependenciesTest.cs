namespace AutoMoxture.Tests.NUnit.WithSut
{
    using AutoFixture;
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
