# Movie Library Web Application

## Description

This is a Movie Library web application developed using C#, ASP.NET Core, and Entity Framework with SQL Server. The application allows users to manage their personal movie library, including adding, editing, and deleting movies. It also provides features to filter movies based on categories, availability, and IMDB ratings, and to suggest movies that have not been watched yet.

## Technologies Used

- **C#:** For server-side logic and application functionality.
- **ASP.NET Core:** For building the web application using the Model-View-Controller (MVC) architectural pattern.
- **Entity Framework:** For data access and ORM (Object-Relational Mapping).
- **SQL Server:** For storing and managing the application's data.
- **HTML/CSS:** For structuring and styling web pages.
- **JavaScript:** For enhancing interactivity and user experience.

## Tools Used

- **Visual Studio:** For code editing and development.
- **SQL Server Management Studio (SSMS):** For managing the SQL Server database, including executing SQL scripts and configuring database settings.

## Features

#### Movie Management

The application allows users to add new movies with detailed information. When adding a movie, users need to provide several details: the title of the movie, its release date, a list of the main actors, the genre of the movie (such as Action, Drama, Comedy, etc.), and the platforms where the movie is available (like File, Netflix, Prime Video, Disney+, etc.). Additionally, users can input the movie's IMDB rating and add an image of the movie. Users can also indicate whether they have seen the movie or not.

Once movies are added, users can also update or delete them as needed. This means that if a movie's details change or if a movie needs to be removed from the library, users can easily make those adjustments.

#### Category Management

Users have the ability to manage movie categories effectively. They can create new categories, update existing ones, or delete categories that are no longer needed. Categories might include genres such as Action, Drama, Comedy, Horror, and more.

#### Platform Management

Users can manage the availability options for movies by creating new availability platforms, updating existing ones, or deleting options that are no longer relevant. Examples of availability platforms include File, Netflix, Amazon Prime Video, Disney+, HBO, etc.

#### Actor Management

Users can also manage actors associated with movies. This includes the ability to create new actor entries, update existing actor details, and delete actors if necessary. The list of actors for each movie can be maintained and updated as required, ensuring accurate representation of all individuals involved in the movies.

#### Movie Viewing

The application provides a comprehensive view of all movies in the library. Users can filter movies based on several criteria, such as category (e.g., Action, Drama), availability (e.g., Netflix, Prime Video), IMDB rating, release date, or the date they watched the movie.

When viewing a movie, users can see detailed information, including the movie's image, all relevant details, and a list of the main actors. This helps users get a complete overview of each movie in their library.

#### Movie Suggestions

The application features a movie suggestion system. Users can receive a random movie suggestion based on the highest IMDB rating that they have not yet watched. Additionally, the application highlights new movie releases, showcasing the most recent additions to the library.

#### CRUD Operations

For all entities within the application, including movies, categories, actors, and platforms options, users can perform full CRUD operations. This means they can create new entries, read and view details, update existing entries, and delete any that are no longer needed.

## How to Run

Follow these steps to set up and run the application:

1. **Download the Application:**
   - Clone the repository to your local machine using the following command:
     ```bash
     git clone https://github.com/nikolaoskor/movie-library.git
     ```
   - Alternatively, download the project as a ZIP file from the repository and extract it to your desired location.

2. **Install SQL Server and SQL Server Management Studio (SSMS):**
   - Download and install [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) if you haven't already.
   - Download and install [SQL Server Management Studio (SSMS)](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) to manage your SQL Server database.

3. **Create the Database:**
   - Open SQL Server Management Studio (SSMS).
   - Connect to your SQL Server instance.
   - Open the `SQL_Query.SQL` file provided in the project.
   - Execute the script to create the database, tables, and insert initial data.

4. **Configure the Application:**
   - Open the `moviesLibraryDB.cs` file located in the `Models` folder of your project.
   - Locate the `OnConfiguring` method and update the connection string to match your SQL Server setup. For example:
     ```csharp
     protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
     {
         #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
         optionsBuilder.UseSqlServer("Data Source=YOUR_SERVER_NAME;Initial Catalog=moviesLibraryDB;Integrated Security=True;Encrypt=False");
     }
     ```
   - Replace `YOUR_SERVER_NAME` with the name of your SQL Server instance.

5. **Run the Application:**
   - Open the project in Visual Studio.
   - Set the desired build configuration (e.g., Debug).
   - Press `F5` or click on the "Start" button to run the application.

6. **Access the Application:**
   - Open a web browser and navigate to `http://localhost` to view and interact with your Movie Library application. If you are using a different port, adjust the URL accordingly. For example, if your application is running on port 5001, you would navigate to `http://localhost:5001`.
