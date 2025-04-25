namespace ei8.Avatar.Installer.Common.Test;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("HelloWorld", "hello_world")]
    //[InlineData("MyHTTPServer", "my_http_server")]
    [InlineData("XMLParser", "xml_parser")]
    //[InlineData("NeurULServer", "neurul_server")]
    [InlineData("SimpleTest", "simple_test")]
    public void ToSnakeCase_ShouldConvertToSnakeCase(string input, string expected)
    {
        // Act
        var result = input.ToSnakeCase();

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("HelloWorld", "HELLO_WORLD")]
    //[InlineData("MyHTTPServer", "MY_HTTP_SERVER")]
    [InlineData("XMLParser", "XML_PARSER")]
    //[InlineData("NeurULServer", "NEURUL_SERVER")]
    [InlineData("SimpleTest", "SIMPLE_TEST")]
    public void ToMacroCase_ShouldConvertToMacroCase(string input, string expected)
    {
        // Act
        var result = input.ToMacroCase();

        // Assert
        Assert.Equal(expected, result);
    }
}
