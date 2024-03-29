namespace AutoMoxture.Testing;

public class ServiceWithDependencies
{
    private readonly IDependency1 dependency1;
    private readonly IDependency2 dependency2;
    private readonly IDependency3 dependency3;

    public ServiceWithDependencies(IDependency1 dependency1, IDependency2 dependency2, IDependency3 dependency3)
    {
        this.dependency1 = dependency1;
        this.dependency2 = dependency2;
        this.dependency3 = dependency3;
    }

    public string Concat(string prefix)
    {
        return prefix
            + this.dependency1.GetString()
            + this.dependency2.GetString()
            + this.dependency3.GetString();
    }
}
