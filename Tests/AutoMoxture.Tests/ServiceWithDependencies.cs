namespace AutoMoxture.Tests
{
    public class ServiceWithDependencies
    {
        private readonly IDependency1 _dependency1;
        private readonly IDependency2 _dependency2;
        private readonly IDependency3 _dependency3;

        public ServiceWithDependencies(IDependency1 dependency1, IDependency2 dependency2, IDependency3 dependency3)
        {
            _dependency1 = dependency1;
            _dependency2 = dependency2;
            _dependency3 = dependency3;
        }

        public string Concat(string prefix)
        {
            return prefix
                + _dependency1.GetString()
                + _dependency2.GetString()
                + _dependency3.GetString();
        }
    }
}
