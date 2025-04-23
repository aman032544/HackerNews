# Hacker News API

This is the backend API for the Hacker News app built using ASP.NET Core. It fetches top stories from the Hacker News public API and returns them to the frontend.

## How to Run

1. Open the project in Visual Studio or Visual Studio Code.

2. Run the project:

   ```
   dotnet run
   ```

3. The API will be available at:

   ```
   https://localhost:5001/api/story
   ```

## Features

- Fetches top stories from Hacker News
- Returns structured JSON data
- Includes simple caching and exception handling

## Testing

To run unit tests:

```
dotnet test
```
