namespace AutoMoxture.XUnit;

using System;

using AutoFixture;
using AutoFixture.AutoMoq;

using Moq;

/// <summary>
/// Base class to get started with AutoFixture and AutoMoq.
/// </summary>
public abstract class AutoMoxtureTest : Fixture
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMoxtureTest"/> class.
    /// </summary>
    protected AutoMoxtureTest()
    {
        this.Customize(new AutoMoqCustomization());
    }

    /// <summary>
    /// Freezes the Mock of the given type.
    /// Alias method for Freeze&lt;Mock&lt;T&gt;&gt;.
    /// </summary>
    /// <typeparam name="T">The type of the Mock.</typeparam>
    /// <returns>A frozen Mock of the given type.</returns>
    public Mock<T> Mock<T>()
        where T : class
    {
        return this.Freeze<Mock<T>>();
    }
}

/// <summary>
/// Base class to get started with AutoFixture and AutoMoq.
/// </summary>
/// <typeparam name="TSut">The type of the system under test (SUT).</typeparam>
public abstract class AutoMoxtureTest<TSut> : AutoMoxtureTest
{
    /// <summary>
    /// Gets an instance of the SUT.
    /// </summary>
    protected TSut Sut => this.Freeze<TSut>();

    /// <summary>
    /// Sets a creation function for the SUT.
    /// </summary>
    protected Func<TSut> SutFactory
    {
        set => this.Register<TSut>(value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMoxtureTest{TSut}"/> class.
    /// </summary>
    protected AutoMoxtureTest()
    {
        this.SutFactory = () => this.Build<TSut>().OmitAutoProperties().Create();
    }
}