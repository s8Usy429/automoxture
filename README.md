# AutoMoxture
AutoMoxture provides a convenient base class to work with AutoFixture and AutoMoq so you can inherit all your test classes from it :

```cs
public class DemoServiceTests : AutoMoxtureTest<DemoService>
{
	...
}
```

AutoMoxture will create and expose the `Sut` property out of the box :

```cs
public class DemoServiceTests : AutoMoxtureTest<DemoService>
{
	...

	// Act
	var response = Sut.DemoMethod();

	...
}
```

AutoMoxture also inherits from AutoFixture.Fixture so AutoFixture methods are directly accessible :

```cs
public class DemoServiceTests : AutoMoxtureTest<DemoService>
{
	...

	// Create a string
	string lastName = Create<string>();

	// Create multiple strings
	IEnumerable<string> multipleLastNames = CreateMany<string>();

	// Instruct AutoFixture to always return the same string in the future
	string frozenLastName = Freeze<string>();

	// Create a custom builder for the class People
	var peopleBuilder = Build<People>().With(b => b.LastName, "Cool Last Name");

	...
}
```

AutoMoxture also introduces an alias method `Mock<T>` to quickly freeze a Mock :

```cs
public class DemoServiceTests : AutoMoxtureTest<DemoService>
{
	...

	// Regular way to freeze a Mock
	var mockOfDependency = Freeze<Mock<IDependencyOfDemoService>>();
	
	// Shorter way to freeze a Mock
	var mockOfDependency = Mock<IDependencyOfDemoService>()

	...
}
```

It is possible to customize the Fixture in your constructor.

```cs
public class DemoServiceTests : AutoMoxtureTest
{
    public class DemoServiceTests()
    {
        Customize<TheTypeToCustomize>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
    }
}
```

This will apply to all the tests in the class.
If you don't want this behavior, call the Customize method directly inside your test method.

```cs
public class DemoServiceTests : AutoMoxtureTest
{
	public void Test1()
	{
		...
		Customize<TheTypeToCustomize>(c => c.FromFactory(new MethodInvoker(new GreedyConstructorQuery())));
		...
	}
}
```
