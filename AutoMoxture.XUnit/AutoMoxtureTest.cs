#pragma warning disable SA1402 // File may only contain a single type.

namespace AutoMoxture.XUnit
{
    using AutoFixture;
    using AutoFixture.AutoMoq;

    /// <summary>
    /// Base class to get started with AutoFixture and Automoq.
    /// </summary>
    public abstract class AutoMoxtureTest
    {
        /// <summary>
        /// Gets an instance of AutoFixture registered with AutoMoq.
        /// </summary>
        protected IFixture Fixture { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMoxtureTest"/> class.
        /// </summary>
        protected AutoMoxtureTest()
        {
            this.Fixture = new Fixture().Customize(new AutoMoqCustomization());
        }
    }

    /// <summary>
    /// Base class to get started with AutoFixture and Automoq.
    /// </summary>
    /// <typeparam name="TSut">The type of the system under test (SUT).</typeparam>
    public abstract class AutoMoxtureTest<TSut> : AutoMoxtureTest
    {
        /// <summary>
        /// Gets a frozen instance of SUT.
        /// </summary>
        /// <returns>A frozen instance of the SUT.</returns>
        protected TSut Sut => this.Fixture.Freeze<TSut>();
    }
}

#pragma warning restore SA1402