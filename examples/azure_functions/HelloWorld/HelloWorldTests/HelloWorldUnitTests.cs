using System;
using Xunit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging.Abstractions;

namespace HelloWorldTests
{
    public class HelloWorldUnitTests
    {
        [Fact]
        public void TestHelloNameFunctionSuccessQueryString()
        {
            var httpContext = new DefaultHttpContext();
            var queryStringValue = "World";
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
                (
                    new System.Collections.Generic.Dictionary<string, StringValues>()
                    {
                        { "name", queryStringValue }
                    }
                )
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = HelloWorld.HelloName.Run(request, logger);
            response.Wait();

            // Check that we get an OK response
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);

            // Check that the contents of the response are what we expect
            var result = (OkObjectResult)response.Result;
            string greeting = "Hello, World";
           
            Assert.Equal(greeting, result.Value);
        }

        [Fact]
        public void TestHelloNameFunctionFailureNoQueryStringOrBody()
        {
            var httpContext = new DefaultHttpContext();
            var request = new DefaultHttpRequest(new DefaultHttpContext());
            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = HelloWorld.HelloName.Run(request, logger);

            // Check that we get a bad response
            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);

            // Check that the error message is correct
            var result = (BadRequestObjectResult)response.Result;
            Assert.Equal("Please pass a name on the query string or in the request body", result.Value);
        }

        [Fact]
        public void TestHelloNameFunctionFailureNoNameInQueryString()
        {
            var httpContext = new DefaultHttpContext();
            var queryStringValue = "World";
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
                (
                    new System.Collections.Generic.Dictionary<string, StringValues>()
                    {
                        { "not-name", queryStringValue }
                    }
                )
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = HelloWorld.HelloName.Run(request, logger);
            response.Wait();

            // Check that we get an OK response
            Assert.IsAssignableFrom<BadRequestObjectResult>(response.Result);

            // Check that the error message is correct
            var result = (BadRequestObjectResult)response.Result;
            Assert.Equal("Please pass a name on the query string or in the request body", result.Value);
        }
    }
}
