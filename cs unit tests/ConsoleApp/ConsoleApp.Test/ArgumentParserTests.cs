using ConsoleApp.Lib;

namespace ConsoleApp.Test;

public class ArgumentParserTests
{
    [Theory]
    [InlineData("-as", true, false, false, false)]
    [InlineData("--a='asd'", false, true, false, true)]
    [InlineData("--", false, false, true, false)]
    public void TrySingleParse_AllCases_ReturnTrue(
        string arg, 
        bool isFlagExpected, 
        bool isDoubleFlagExpected,
        bool isSkipExpected,
        bool hasValueExpected)
    {

        Assert.True(ArgumentParser.TrySingleParse(
            arg,
            out bool flagActual,
            out bool doubleFlagActual,
            out bool isSkipActual,
            out bool hasValueActual));

        Assert.Equal(isFlagExpected,        flagActual);
        Assert.Equal(isDoubleFlagExpected,  doubleFlagActual);
        Assert.Equal(isSkipExpected,        isSkipActual);
        Assert.Equal(hasValueExpected,      hasValueActual);
    }


    [Theory]
    [InlineData("", false, false, false, false)]
    [InlineData(null, false, false, false, false)]
    public void TrySingleParse_AllCases_ReturnFalse(
    string arg,
    bool isFlagExpected,
    bool isDoubleFlagExpected,
    bool isSkipExpected,
    bool hasValueExpected)
    {

        Assert.False(ArgumentParser.TrySingleParse(
            arg,
            out bool flagActual,
            out bool doubleFlagActual,
            out bool isSkipActual,
            out bool hasValueActual));

        Assert.Equal(isFlagExpected, flagActual);
        Assert.Equal(isDoubleFlagExpected, doubleFlagActual);
        Assert.Equal(isSkipExpected, isSkipActual);
        Assert.Equal(hasValueExpected, hasValueActual);
    }




    [Theory]
    [InlineData("--test1=\"abc\"", "test1", "abc")]
    [InlineData("--test2=abcd", "test2", "abcd")]
    public void TryParseArgDictEntry_AllCases_ReturnTrue(string arg, string expectedKey, string expectedValue)
    {
        bool result = ArgumentParser.TryParseArgDictEntry(arg, out string actualKey, out string actualValue);

        Assert.True(result, "WUT!");
        Assert.Equal(expectedKey, actualKey);
        Assert.Equal(expectedValue, actualValue);
    }

    [Theory]
    [InlineData("a")]
    [InlineData("w")]
    [InlineData("e")]
    [InlineData("")]
    public void HasArgument_AllCases_ReturnTrue(string argument)
    {
        var p = new ArgumentParser("echo -a -we install");

        Assert.True(p.HasArgument(argument));
    }

    [Theory]
    [InlineData("d")]
    [InlineData("c")]
    [InlineData("n")]
    public void HasArgument_AllCases_ReturnFalse(string argument)
    {
        var p = new ArgumentParser("echo -a -we install");

        Assert.False(p.HasArgument(argument));
    }


    [Theory]
    [InlineData("stuff", "Hello")]
    [InlineData("stsuff", "")]
    [InlineData("stuff2", "World!")]
    public void GetArgument_LongArgumentsTest(string key, string value)
    {
        var p = new ArgumentParser("asd -a --stuff=\"Hello\" --stuff2='World!'");

        Assert.Equal(value, p.GetArgument(key));
    }


    [Fact]
    public void ArgumentParse_LongArgumentsTest2()
    {
        var p = new ArgumentParser(new string[] {"-a", "--help", "--testa=\"Hello World\"", "--testb=Hello another WORLD!!!"});

        Assert.True(p.HasArgument("help"));
        Assert.Equal("Hello World", p.GetArgument("testa"));
        Assert.Equal("Hello another WORLD!!!", p.GetArgument("testb"));
    }


    [Fact]
    public void TerSplit_mk2_Tests()
    {
        var p = new ArgumentParser("");
        string[] argExpected = new string[] { "-a", "--help", "--testa=Hello World", "--testb=Hello", "another", "WORLD!!!" };
        string argsString = "-a --help --testa=\"Hello World\" --testb=Hello another WORLD!!!";
        string[] argActual = ArgumentParser.TerSplit_mk2(argsString);

        for (int i = 0; i < argExpected.Length; i++)
        {
            Console.WriteLine($"Expected: {argExpected[i]}, Actual: {argActual[i]}");
            Assert.Equal(argExpected[i], argActual[i]);
        }


        Assert.Equal(argExpected.Length, argActual.Length);

    }







}
