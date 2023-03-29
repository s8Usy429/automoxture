namespace AutoMoxture.Tests.NUnit.WithSut
{
    using AutoMoxture.NUnit;
    using FluentAssertions;
    using global::NUnit.Framework;

    [TestFixture]
    public class SutFactoryTest : AutoMoxtureTest<SomeSut>
    {
        [Test]
        public void SutFactory_WhenNotReassigned_ShouldProduceSameValue()
        {
            // Arrange
            Guid id1;
            Guid id2;

            // Act
            id1 = this.Sut.Id;
            id2 = this.Sut.Id;

            // Assert
            id1.Should().Be(id2);
        }

        [Test]
        public void SutFactory_WhenReassigned_ShouldProduceDifferentValue()
        {
            // Arrange
            Guid id1 = this.Sut.Id;

            // Act
            this.SutFactory = () => new SomeSut { Id = Guid.NewGuid() };

            // Assert
            Guid id2 = this.Sut.Id;
            id1.Should().NotBe(id2);
        }

        [Test]
        public void SutFactory_AfterReassigned_ShouldProduceSameValue()
        {
            // Arrange
            Guid id1 = this.Sut.Id;

            // Act
            this.SutFactory = () => new SomeSut { Id = Guid.NewGuid() };

            // Assert
            Guid id2 = this.Sut.Id;
            Guid id3 = this.Sut.Id;
            id1.Should().NotBe(id2);
            id2.Should().Be(id3);
        }
    }
}
