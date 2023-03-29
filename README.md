# AutoMoxture
AutoMoxture provides a convenient base test class to work with AutoFixture and AutoMoq so you can inherit all your test classes from it.

## Back-story

Consider you would like to write unit tests for this class :
```cs
public class ServiceWithDependencies
{
    private readonly IDependency1 dependency1;
    private readonly IDependency2 dependency2;
    private readonly IDependency3 dependency3;
    private readonly IDependency4 dependency4;
    private readonly IDependency5 dependency5;

    public ServiceWithDependencies(
        IDependency1 dependency1,
        IDependency2 dependency2,
        IDependency3 dependency3,
        IDependency3 dependency4,
        IDependency3 dependency5)
    {
        this.dependency1 = dependency1;
        this.dependency2 = dependency2;
        this.dependency3 = dependency3;
        this.dependency4 = dependency4;
        this.dependency5 = dependency5;
    }

    public string Concat(string prefix)
    {
        return prefix
            + this.dependency1.GetString()
            + this.dependency2.GetString()
            + this.dependency3.GetString()
            + this.dependency4.GetString()
            + this.dependency5.GetString();
    }
}
```

Without AutoMoxture, you would end up writing this at some point :
```cs
public class ServiceWithDependenciesTests
{
    ...

    // Arrange
    var mockDependency1 = new Mock<IDependency1>();
    var mockDependency2 = new Mock<IDependency2>();
    var mockDependency3 = new Mock<IDependency3>();
    var mockDependency4 = new Mock<IDependency4>();
    var mockDependency5 = new Mock<IDependency5>();
    var sut = new ServiceWithDependencies(
        mockDependency1.Object,
        mockDependency2.Object,
        mockDependency3.Object,
        mockDependency4.Object,
        mockDependency5.Object);
    
    ...
}
```

or this :
```cs
public class ServiceWithDependenciesTests
{
    private AutoFixture.Fixture Fixture { get; }

    ...

    // Arrange
    var mockDependency1 = this.Fixture.Create<Mock<IDependency1>>();
    var mockDependency2 = this.Fixture.Create<Mock<IDependency2>>();
    var mockDependency3 = this.Fixture.Create<Mock<IDependency3>>();
    var mockDependency4 = this.Fixture.Create<Mock<IDependency4>>();
    var mockDependency5 = this.Fixture.Create<Mock<IDependency5>>();
    var sut = new ServiceWithDependencies(
        mockDependency1.Object,
        mockDependency2.Object,
        mockDependency3.Object,
        mockDependency4.Object,
        mockDependency5.Object);

    ...
}
```

It's quite boring to write and it gets even worse if your class have more dependencies.  
Also, if you add and/or remove dependencies, it won't build anymore and you have to go fix your tests.

## AutoMoxture

Turns out, AutoFixture and AutoMoq have already solved this problem :
```cs
var fixture = new AutoFixture.Fixture();
fixture.Customize(new AutoMoqCustomization());
var sut = fixture.Create<ServiceWithDependencies>();
```

That's more or less what AutoMoxture is doing in its base class so you won't have to write this boilerplate code.

1. Start by inheriting AutoMoxtureTest :
    ```cs
    public class ServiceWithDependenciesTests : AutoMoxtureTest<ServiceWithDependencies>
    {
    }
    ```

2. AutoMoxture will create and expose the `Sut` property out of the box :
    ```cs
    public class ServiceWithDependenciesTests : AutoMoxtureTest<ServiceWithDependencies>
    {
        ...

        // Act
        var response = this.Sut.Concat(...);

        ...
    }
    ```

3. AutoMoxture also exposes an instance of AutoFixture so regular/usual AutoFixture methods are accessible :
    ```cs
    public class ServiceWithDependenciesTests : AutoMoxtureTest<ServiceWithDependencies>
    {
        ...
        
        // Arrange
        var prefix = this.Fixture.Create<string>();

        // Act
        var response = this.Sut.Concat(prefix);

        ...
    }
    ```

4. AutoMoxture also introduces an alias method `Mock<T>` to quickly freeze a Mock :
    ```cs
    public class ServiceWithDependenciesTests : AutoMoxtureTest<ServiceWithDependencies>
    {
        ...

        // Arrange
        var prefix = this.Fixture.Create<string>();

        var dependentString = this.Fixture.Create<string>();
        this.Fixture.Mock<Dependency1>()
            .Setup(m => m.GetString())
            .Returns(dependentString);

        // Act
        var response = this.Sut.Concat(prefix);

        ...
    }
    ```

5. Overall, the test might end up looking like this :
    ```cs
    public class ServiceWithDependenciesTests : AutoMoxtureTest<ServiceWithDependencies>
    {
        ...

        // Arrange
        var prefix = this.Fixture.Create<string>();

        var dependentString = this.Fixture.Create<string>();
        this.Fixture.Mock<Dependency1>()
            .Setup(m => m.GetString())
            .Returns(dependentString);

        // Act
        var response = this.Fixture.Sut.Concat(prefix);

        // Assert
        response.Should().Contain(dependentString);

        ...
    }
    ```

## Customize the fixture
It is possible to customize the Fixture in your constructor.
```cs
public class ServiceWithDependenciesTests : AutoMoxtureTest
{
    public class ServiceWithDependenciesTests()
    {
        this.Fixture.Customize<TheTypeToCustomize>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }
}
```

This will apply to all the tests in the class.  
If you don't want this behavior, call the Customize method directly inside your test method.
```cs
public class ServiceWithDependenciesTests : AutoMoxtureTest
{
    public void Test1()
    {
        this.Fixture.Customize<TheTypeToCustomize>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }
}
```

## Customize the SUT factory
Sometimes you need to control the way the SUT is created.
More likely, you may have a single test that needs a particular SUT setup.
You can provide a neww factory at any time like this:
```cs
// Do stuff with the old/regular SUT
var sutBeforeChange = this.Sut;

// Provide a new factory
this.SutFactory = () => new TheSutType();

// Do stuff with the new/latest SUT
var sutAfterChange = this.Sut;
```

Note that the SUT will still keep its value after being reassigned:
```cs
this.SutFactory = () => new TheSutType();
var sutAfterChange1 = this.Sut;
var sutAfterChange2 = this.Sut;
// Then sutAfterChange1 == sutAfterChange2
```