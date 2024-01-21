# HackerNewsApi

The hacker news api, is a service that returns the last best N-stories from https://hacker-news.firebaseio.com

The architecture of the application is based on the [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/) using the minimal API approach.

The solution template is broken into 2 projects:

### HackerNews.Api

ASP.NET Web API project is an entry point to the application. The main features are in the Features folder.

### HackerNews.Tests

This projects contains tests for the application.

## How to run the app

1. Install the latest [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
2. Clone the repository
```bash
git clone https://github.com/seregakup/HackerNewsSergheiCupelschii.git
 ```
3. Navigate to `src/HackerNews.Api` and run `dotnet run` to launch the back end (ASP.NET Core Web API) or via
```bash 
 dotnet run --project src/HackerNews.Api/HackerNews.Api.csproj
```

### Build, test application

CLI commands executed from the root folder.

```bash
# build
dotnet build

# run
dotnet run --project src/HackerNews.Api/HackerNews.Api.csproj

# run unit tests
dotnet test src/HackerNews.Tests/HackerNews.Tests.csproj
```

## How to get the data
Using Swagger UI http://localhost:5241/swagger/index.html

Or
```bash
curl -X 'GET' 'http://localhost:5241/api/news-management/best-news?amountOfItems=25' -H 'accept: application/json'
```
AmountOfItems is an optional parameter. 
The default value for the amountOfItems is 25. 
AmountOfItems should be between 1 and 500. I added this limitation because of the Hacker News API returns only up to 500 stories. 

Attention! The first request for 500 stories will take about 30 seconds, because of using SemapthoreSlim to avoid race condition.
The next requests will much be faster because of using cache.

## Cache
For the performance reasons, the application uses in memory cache for the stories to avoid overloading of the Hacker News API.
The good approach is to configure setting for the InMemoryCache, but for the simplicity I used the default settings.

## Next features

There are a bunch of features and good practices to enhance the service, I will add them as soon as possible and based on the necessity

 - To avoid overloading of the Hacker News API, we can add the Polly library to control the number of requests per second.
 - For more readable code, we can implement Result pattern.
 - For secure reasons, we can add authorization and authentication.
 - In the case of adding new features, the validation of the input data becomes more complex hence the good solution is to use FluentValidation library.
 - The tests can be improved and extended.

## Built With

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Carter](https://github.com/CarterCommunity/Carter)
- [MediatR](https://github.com/jbogard/MediatR)

## Authors

- Serghei Cupelschii - [seregakup]