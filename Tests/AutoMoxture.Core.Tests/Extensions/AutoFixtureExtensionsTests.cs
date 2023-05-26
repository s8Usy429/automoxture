namespace AutoMoxture.Core.Tests;

using System.Linq;

using AutoFixture;
using AutoFixture.AutoMoq;

using AutoMoxture.NUnit;

using FluentAssertions;

using global::NUnit.Framework;

public class AutoFixtureExtensionsTests : AutoMoxtureTest<Fixture>
{
    public AutoFixtureExtensionsTests()
    {
        // Do this, otherwise the created Fixture comes with AutoMoqCustomization.
        this.SutFactory = () => new Fixture();
    }

    [Test]
    public void EnableAutoMoq_ShouldContainAutoMoqCustomizations([Values] bool multipleCalls)
    {
        this.Sut.EnableAutoMoq();

        if (multipleCalls)
        {
            this.Sut.EnableAutoMoq();
            this.Sut.EnableAutoMoq();
        }

        this.Sut.Customizations.OfType<MockPostprocessor>().Should().ContainSingle();
        this.Sut.ResidueCollectors.OfType<MockRelay>().Should().ContainSingle();
    }

    [Test]
    public void DisableAutoMoq_ShouldNotContainAutoMoqCustomizations([Values] bool wasEnabled, [Values] bool multipleCalls)
    {
        if (wasEnabled)
        {
            this.Sut.Customize(new AutoMoqCustomization());
        }

        this.Sut.DisableAutoMoq();
        if (multipleCalls)
        {
            this.Sut.DisableAutoMoq();
            this.Sut.DisableAutoMoq();
        }

        this.Sut.Customizations.OfType<MockPostprocessor>().Should().BeEmpty();
        this.Sut.ResidueCollectors.OfType<MockRelay>().Should().BeEmpty();
    }
}