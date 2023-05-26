namespace AutoMoxture.NUnit;

using System;

using AutoFixture;
using AutoFixture.AutoMoq;

using global::NUnit.Framework;

/// <summary>
/// Base class to get started with AutoFixture and Automoq.
/// </summary>
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public abstract class AutoMoxtureTest
{
    private readonly Lazy<IFixture> lazyFixture;

    /// <summary>
    /// Gets an instance of AutoFixture registered with AutoMoq.
    /// </summary>
    protected IFixture Fixture => this.lazyFixture.Value;

    private bool enableAutoMoq;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMoxtureTest"/> class.
    /// </summary>
    protected AutoMoxtureTest()
        : this(enableAutoMoq: true)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMoxtureTest"/> class.
    /// </summary>
    /// <param name="enableAutoMoq">A boolean indicating whether to enable AutoMoq customizations.</param>
    protected AutoMoxtureTest(bool enableAutoMoq)
    {
        this.enableAutoMoq = enableAutoMoq;

        this.lazyFixture = new Lazy<IFixture>(
            () =>
            {
                var fixture = new Fixture();
                if (this.enableAutoMoq)
                {
                    fixture.Customize(new AutoMoqCustomization());
                }

                return fixture;
            },
            isThreadSafe: false);
    }

    /// <summary>
    /// Enable AutoMoq customizations.
    /// </summary>
    protected void EnableAutoMoq()
    {
        if (this.lazyFixture.IsValueCreated)
        {
            this.Fixture.EnableAutoMoq();
        }
        else
        {
            this.enableAutoMoq = true;
        }
    }

    /// <summary>
    /// Disable AutoMoq customizations.
    /// </summary>
    protected void DisableAutoMoq()
    {
        if (this.lazyFixture.IsValueCreated)
        {
            this.Fixture.DisableAutoMoq();
        }
        else
        {
            this.enableAutoMoq = false;
        }
    }
}

/// <summary>
/// Base class to get started with AutoFixture and Automoq.
/// </summary>
/// <typeparam name="TSut">The type of the system under test (SUT).</typeparam>
public abstract class AutoMoxtureTest<TSut> : AutoMoxtureTest
{
    /// <summary>
    /// Gets an instance of the SUT.
    /// </summary>
    protected TSut Sut => this.Fixture.Freeze<TSut>();

    /// <summary>
    /// Sets a creation function for the SUT.
    /// </summary>
    protected Func<TSut> SutFactory
    {
        set => this.Fixture.Register<TSut>(value);
    }
}