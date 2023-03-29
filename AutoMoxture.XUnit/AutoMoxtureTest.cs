#pragma warning disable SA1402  // File may only contain a single type.
#pragma warning disable S2376  // Replace set-only property by a method.

namespace AutoMoxture.XUnit
{
    using System;
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
        private Lazy<TSut> lazySut;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMoxtureTest{TSut}"/> class.
        /// </summary>
        protected AutoMoxtureTest()
        {
            this.SutFactory = () => this.Fixture.Freeze<TSut>();
        }

        /// <summary>
        /// Gets an instance of the SUT.
        /// </summary>
        protected TSut Sut => this.lazySut.Value;

        /// <summary>
        /// Sets a creation function for the SUT.
        /// </summary>
        protected Func<TSut> SutFactory
        {
            set
            {
                this.lazySut = new Lazy<TSut>(value);
            }
        }
    }
}

#pragma warning restore SA1402  // File may only contain a single type.
#pragma warning restore S2376  // Replace set-only property by a method.