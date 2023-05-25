namespace AutoMoxture.XUnit.Tests.WithoutSut
{
    using AutoFixture;
    using AutoMoxture.Testing;
    using AutoMoxture.XUnit;
    using FluentAssertions;
    using Xunit;

    public class ServiceWithDependenciesTest : AutoMoxtureTest
    {
        [Fact]
        public void AutoMoxtureTest_ServiceWithDependencies()
        {
            // Arrange
            string prefix = this.Fixture.Create<string>();
            string demo2 = this.Fixture.Create<string>();
            this.Fixture.Mock<IDependency2>()
                .Setup(s => s.GetString())
                .Returns(demo2);
            var sut = this.Fixture.Create<ServiceWithDependencies>();

            // Act
            var response = sut.Concat(prefix);

            // Assert
            response.Should().Contain(demo2);
        }
    }
}
