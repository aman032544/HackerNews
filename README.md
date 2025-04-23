# Hacker News UI

This is the frontend of the Hacker News app built using Angular. It fetches and displays top stories from the backend API.

## How to Run

1. Install dependencies:

   ```
   npm install
   ```

2. Start the development server:

   ```
   ng serve
   ```

3. Visit the app in your browser:

   ```
   http://localhost:4200
   ```

## Configuration

Make sure to set the correct API URL in `src/environments/environment.ts`:

```
API_BASE_URL: 'http://localhost:5000/api/story'
```

## Features

- Displays top stories from Hacker News
- Search and pagination support
- Basic caching
- Responsive layout

## Testing

To run unit tests:

```
ng test
```


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
