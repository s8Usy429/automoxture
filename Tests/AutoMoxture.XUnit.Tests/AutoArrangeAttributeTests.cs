namespace AutoMoxture.XUnit.Tests;

using System;
using System.ComponentModel;
using System.Linq;
using System.Net;

using FluentAssertions;

using Xunit;

#pragma warning disable S2699
#pragma warning disable S3881
#pragma warning disable SA1128
#pragma warning disable SA1402
#pragma warning disable SA1502
#pragma warning disable SA1649
#pragma warning disable xUnit1026

public abstract class TestContext : IDisposable
{
    public int TestRunCountExpectation { get; }

    public int TestRunCount { get; set; }

    protected TestContext(int count)
    {
        this.TestRunCountExpectation = count;
    }

    public void Dispose()
    {
        this.TestRunCount.Should().Be(this.TestRunCountExpectation);
    }
}

public class TestContext1 : TestContext
{
    public TestContext1() : base(2) { }
}

public class TestContext2 : TestContext
{
    public TestContext2() : base(4) { }
}

public class TestContext3 : TestContext
{
    public TestContext3() : base(Enum.GetValues<HttpStatusCode>().Distinct().Count()) { }
}

public class TestContext4 : TestContext
{
    public TestContext4() : base(2 * Enum.GetValues<HttpStatusCode>().Distinct().Count()) { }
}

public class TestContext5 : TestContext
{
    public TestContext5() : base(2) { }
}

public class TestContext6 : TestContext
{
    public TestContext6() : base(1) { }
}

public class TestContext7 : TestContext
{
    public TestContext7() : base(8) { }
}

public class TestContext8 : TestContext
{
    public TestContext8() : base(1) { }
}

public class TestContext10 : TestContext
{
    public TestContext10() : base(6) { }
}

public class TestContext11 : TestContext
{
    public TestContext11() : base(6) { }
}

public class AutoArrangeAttributeTests :
    IClassFixture<TestContext1>,
    IClassFixture<TestContext2>,
    IClassFixture<TestContext3>,
    IClassFixture<TestContext4>,
    IClassFixture<TestContext5>,
    IClassFixture<TestContext6>,
    IClassFixture<TestContext7>,
    IClassFixture<TestContext8>,
    IClassFixture<TestContext10>,
    IClassFixture<TestContext11>
{
    private readonly TestContext1 testContext1;
    private readonly TestContext2 testContext2;
    private readonly TestContext3 testContext3;
    private readonly TestContext4 testContext4;
    private readonly TestContext5 testContext5;
    private readonly TestContext6 testContext6;
    private readonly TestContext7 testContext7;
    private readonly TestContext8 testContext8;
    private readonly TestContext10 testContext10;
    private readonly TestContext11 testContext11;

    public AutoArrangeAttributeTests(
        TestContext1 testContext1,
        TestContext2 testContext2,
        TestContext3 testContext3,
        TestContext4 testContext4,
        TestContext5 testContext5,
        TestContext6 testContext6,
        TestContext7 testContext7,
        TestContext8 testContext8,
        TestContext10 testContext10,
        TestContext11 testContext11)
    {
        this.testContext1 = testContext1;
        this.testContext2 = testContext2;
        this.testContext3 = testContext3;
        this.testContext4 = testContext4;
        this.testContext5 = testContext5;
        this.testContext6 = testContext6;
        this.testContext7 = testContext7;
        this.testContext8 = testContext8;
        this.testContext10 = testContext10;
        this.testContext11 = testContext11;
    }

    [Theory, AutoArrange]
    [Description("Should be called 2 times")]
    public void Test1(bool param1, [Values] bool param2) => ++this.testContext1.TestRunCount;

    [Theory, AutoArrange]
    [Description("Should be called 4 times")]
    public void Test2([Values] bool param1, [Values] bool param2) => ++this.testContext2.TestRunCount;

    [Theory, AutoArrange]
    [Description("Should be called 61 times (HttpStatusCode has 66 items but has duplicates)")]
    public void Test3([Values] HttpStatusCode httpStatusCode) => ++this.testContext3.TestRunCount;

    [Theory, AutoArrange]
    [Description("Should be called 2 * 61 times")]
    public void Test4([Values] bool param1, [Values] HttpStatusCode httpStatusCode) => ++this.testContext4.TestRunCount;

    [Theory, AutoArrange]
    [Description("Should be called 2 times")]
    public void Test5([Values] bool param1) => ++this.testContext5.TestRunCount;

    [Theory, AutoArrange]
    [Description("Should be called once")]
    public void Test6(bool param1) => ++this.testContext6.TestRunCount;

    [Theory, AutoArrange]
    [Description("Should be called 8 times")]
    public void Test7([Values] bool param1, [Values] bool param2, [Values] bool param3) => ++this.testContext7.TestRunCount;

    [Theory, AutoArrange]
    [Description("Should be called once")]
    public void Test8([Values] int param1) => ++this.testContext8.TestRunCount;

    [Theory]
    [AutoArrange("toto")]
    [AutoArrange("titi")]
    [AutoArrange("tata")]
    [Description("Testing the attribute as multiple test cases")]
    public void Test10(string param1, [Values] bool param2)
    {
        param1.Should().BeOneOf("toto", "titi", "tata");
        ++this.testContext10.TestRunCount;
    }

    [Theory, AutoArrange]
    [AutoArrange]
    [AutoArrange("toto")]
    [AutoArrange("titi")]
    [AutoArrange("tata")]
    [AutoArrange]
    [Description("If both format are used, the empty one is ignored")]
    public void Test11(string param1, [Values] bool param2)
    {
        param1.Should().BeOneOf("toto", "titi", "tata");
        ++this.testContext11.TestRunCount;
    }
}
