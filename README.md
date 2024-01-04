# MyMovies Application

This is a full-stack web application for managing a personal movie library.

## Prerequisites

Before running the application, make sure you have the following environments and tools installed:

- [Node.js](https://nodejs.org/) (version 21.5.0) - JavaScript runtime environment.
- [Vue CLI](https://cli.vuejs.org/) (version 5.0.8) - Standard Tooling for Vue.js Development.
- [.NET 6 SDK](https://dotnet.microsoft.com/download) (version 6.0.417) - Software development kit for building with .NET 6.
- [ASP.NET Core Runtime](https://dotnet.microsoft.com/download) (version 6.0.25) - Runtime for building and running ASP.NET Core applications.

## Getting Started

To get the application running, start with clone the repository to your local machine by

```sh
git clone https://github.com/LukaszSwor/MyMovies.git
```

## Database Setup

This application uses MySQL for the database and Entity Framework Core for ORM with migrations. To set up the database:

1. Ensure you have MySQL installed and running on your machine.

2. Update the `DefaultConnection` string in the `appsettings.json` file within the `mojefilmy-backend` project to match your MySQL server's credentials:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=your_server;port=your_port;database=your_database;user=your_username;password=your_password;"
}
```

Please make sure to secure your database in a production environment and not to use default credentials.

3. Apply Entity Framework migrations to create the database schema:

```sh
cd mojefilmy-backend
dotnet ef migrations add InitialCreate
dotnet ef database update
```

If you do not have Entity Framework CLI tools installed, you can install them using the following command:

```sh
dotnet tool install --global dotnet-ef
```

If you encounter any issues with migrations, make sure that the Entity Framework provider for MySQL is installed and properly configured in your project.

## Start with app


1. Navigate to the front-end directory within the cloned repository.

```sh
cd MyMovies/mojefilmy-frontend
```

2. Install the required Node.js dependencies.

```sh
npm install
```

3. Start the front-end development server.

```sh
npm run serve
```

4. In a separate terminal, navigate to the back-end directory.

```sh
cd /MyMovies/mojefilmy-backend
```

5. Restore the .NET dependencies.

```sh
dotnet restore
```

7. Start the back-end development server.

```sh
dotnet run
```

The front-end will be accessible at `http://localhost:8080`, and the back-end will be available at `http://localhost:5000` or the port specified in the launch settings.


## Application Overview

The application allows users to perform the following operations:

- **Add a new movie**: Enter details such as the title, director, and year.
- **Edit an existing movie**: Update the details of movies in the collection.
- **Delete a movie**: Remove a movie from the collection.
- **List all movies**: Display all movies in the collection.

These operations are supported by a RESTful API backend built with ASP.NET Core and a frontend built with Vue.js.

---

For more information or if you encounter any issues, please open an issue on the GitHub repository.

