#pragma warning disable SA1402 // File may only contain a single type.

namespace AutoMoxture.NUnit
{
    using System;
    using System.Collections.Generic;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using AutoFixture.Dsl;
    using AutoFixture.Kernel;
    using global::NUnit.Framework;
    using Moq;

    /// <summary>
    /// Base class to get started with AutoFixture and Automoq.
    /// </summary>
    [FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
    public abstract class AutoMoxtureTest : Fixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutoMoxtureTest"/> class.
        /// </summary>
        /// <param name="autosetup">Automatically setup mock calls.</param>
        protected AutoMoxtureTest(bool autosetup = false)
        {
            Customize(new AutoMoqCustomization { ConfigureMembers = autosetup });
        }

        #region SpecimenFactory

        /// <summary>
        /// Creates an anonymous variable of the requested type.
        /// </summary>
        /// <typeparam name="T">The type of object to create.</typeparam>
        /// <returns>An anonymous object of type T.</returns>
        public T Create<T>() => SpecimenFactory.Create<T>(this);

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <returns>A sequence of anonymous object of type T.</returns>
        public IEnumerable<T> CreateMany<T>() => SpecimenFactory.CreateMany<T>(this);

        /// <summary>
        /// Creates many anonymous objects.
        /// </summary>
        /// <param name="count">The number of objects to create.</param>
        /// <typeparam name="T">The type of objects to create.</typeparam>
        /// <returns>A sequence of anonymous object of type T.</returns>
        public IEnumerable<T> CreateMany<T>(int count) => SpecimenFactory.CreateMany<T>(this, count);

        #endregion

        #region FixtureFreezer

        /// <summary>
        /// Freezes the type to a single value.
        /// </summary>
        /// <typeparam name="T">The type to freeze.</typeparam>
        /// <returns>The value that will subsequently always be created for T.</returns>
        public T Freeze<T>() => FixtureFreezer.Freeze<T>(this);

        /// <summary>
        /// Freezes the type to a single value.
        /// </summary>
        /// <param name="composerTransformation">A function that customizes a given AutoFixture.Dsl.ICustomizationComposer`1 and returns the modified composer.</param>
        /// <typeparam name="T">The type to freeze.</typeparam>
        /// <returns>The value that will subsequently always be created for T.</returns>
        public T Freeze<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation) => FixtureFreezer.Freeze<T>(this, composerTransformation);

        #endregion

        /// <summary>
        /// Provides a frozen Mock of a type.
        /// </summary>
        /// <typeparam name="T">The type of the Mock.</typeparam>
        /// <returns>A frozen Mock for the type.</returns>
        protected Mock<T> Mock<T>() where T : class => Freeze<Mock<T>>();
    }

    /// <summary>
    /// Base class to get started with AutoFixture and Automoq.
    /// </summary>
    /// <typeparam name="TSut">The type of the Mock.</typeparam>
    public abstract class AutoMoxtureTest<TSut> : AutoMoxtureTest
    {
        /// <summary>
        /// Gets a frozen instance of SUT.
        /// </summary>
        /// <returns>A frozen instance of the SUT.</returns>
        protected TSut Sut => Freeze<TSut>();
    }
}

#pragma warning restore SA1402