# AutoMoxture
AutoMoxture provides a convenient base test class to work with AutoFixture and AutoMoq so you can inherit all your test classes from it.

## Back-story

Consider you would like to write unit tests for this class :
```cs
public class ServiceWithDependencies
{
    private readonly IDependency1 _dependency1;
    private readonly IDependency2 _dependency2;
    private readonly IDependency3 _dependency3;
    private readonly IDependency4 _dependency4;
    private readonly IDependency5 _dependency5;

    public ServiceWithDependencies(
        IDependency1 dependency1,
        IDependency2 dependency2,
        IDependency3 dependency3,
        IDependency3 dependency4,
        IDependency3 dependency5)
    {
        _dependency1 = dependency1;
        _dependency2 = dependency2;
        _dependency3 = dependency3;
        _dependency4 = dependency4;
        _dependency5 = dependency5;
    }

    public string Concat(string prefix)
    {
        return prefix
            + _dependency1.GetString()
            + _dependency2.GetString()
            + _dependency3.GetString()
            + _dependency4.GetString()
            + _dependency5.GetString();
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
    var mockDependency1 = Fixture.Create<Mock<IDependency1>>();
    var mockDependency2 = Fixture.Create<Mock<IDependency2>>();
    var mockDependency3 = Fixture.Create<Mock<IDependency3>>();
    var mockDependency4 = Fixture.Create<Mock<IDependency4>>();
    var mockDependency5 = Fixture.Create<Mock<IDependency5>>();
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
        var response = Sut.Concat(...);

        ...
    }
    ```

3. AutoMoxture also inherits from AutoFixture.Fixture so AutoFixture methods are directly accessible :
    ```cs
    public class ServiceWithDependenciesTests : AutoMoxtureTest<ServiceWithDependencies>
    {
        ...
        
        // Arrange
        var prefix = Create<string>();

        // Act
        var response = Sut.Concat(prefix);

        ...
    }
    ```

4. AutoMoxture also introduces an alias method `Mock<T>` to quickly freeze a Mock :
    ```cs
    public class ServiceWithDependenciesTests : AutoMoxtureTest<ServiceWithDependencies>
    {
        ...

        // Arrange
        var prefix = Create<string>();

        var dependentString = Create<string>();
        Mock<Dependency1>()
            .Setup(m => m.GetString())
            .Returns(dependentString);

        // Act
        var response = Sut.Concat(prefix);

        ...
    }
    ```

5. Overall, the test might end up looking like this :
    ```cs
    public class ServiceWithDependenciesTests : AutoMoxtureTest<ServiceWithDependencies>
    {
        ...

        // Arrange
        var prefix = Create<string>();

        var dependentString = Create<string>();
        Mock<Dependency1>()
            .Setup(m => m.GetString())
            .Returns(dependentString);

        // Act
        var response = Sut.Concat(prefix);

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
        Customize<TheTypeToCustomize>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
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
        Customize<TheTypeToCustomize>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }
}
```