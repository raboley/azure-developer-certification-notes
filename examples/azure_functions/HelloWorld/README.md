# Azure Functions using Visual Studio

To create an azure function using visual studio it is best to do so on windows. Either way ensure that you have the azure dev tools by starting the visual studio installer and making sure that azure is checked. If it is you should be able to select azure function as a project template when creating a new project.

Start with creating a function project with an HTTP triggered function.

It will default with a function that will return "hello <string>" based on what is passed in for name on either the function body or query string.

## Adding unit testing for Azure functions

To add unit testing add a new project of type xUnit to the azure function solution.

Right click the new project and select manage nuget packages. click over to browse and search for mvc and download `Microsoft.AspNet.Mvc` which will allow us to create a mock http environment to test the http triggers. 


Next add a reference to the azure function project by clicking the dependencies under HelloWorldTests project and choosing to add a reference to the azure function project.

After adding the reference open the unittest1.cs and add code something like this:

``` csharp
[Fact]
        public void TestWatchFunctionSuccess()
        {
            var httpContext = new DefaultHttpContext();
            var queryStringValue = "abc";
            var request = new DefaultHttpRequest(new DefaultHttpContext())
            {
                Query = new QueryCollection
                (
                    new System.Collections.Generic.Dictionary<string, StringValues>()
                    {
                        { "model", queryStringValue }
                    }
                )
            };

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");

            var response = WatchPortalFunction.WatchInfo.Run(request, logger);
            response.Wait();

            // Check that we get an OK response
            Assert.IsAssignableFrom<OkObjectResult>(response.Result);

            // Check that the contents of the response are what we expect
            var result = (OkObjectResult)response.Result;
            dynamic watchinfo = new { Manufacturer = "Abc", CaseType = "Solid", Bezel = "Titanium", Dial = "Roman", CaseFinish = "Silver", Jewels = 15 };
            string watchInfo = $"Watch Details: {watchinfo.Manufacturer}, {watchinfo.CaseType}, {watchinfo.Bezel}, {watchinfo.Dial}, {watchinfo.CaseFinish}, {watchinfo.Jewels}";
            Assert.Equal(watchInfo, result.Value);
        }
```

This will create an http context, send a request with a query string that has one property called model and gives that value abc. It then checks to ensure you get a 200 response, and checks all the properties on the response to ensure they are what is expected.
