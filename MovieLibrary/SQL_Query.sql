-- Create the database
CREATE DATABASE moviesLibraryDB;

--------------------------------------------------------------------------------------

-- Create Categories Table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) NOT NULL
);

-- Create Availabilities Table
CREATE TABLE Availabilities (
    AvailabilityID INT PRIMARY KEY IDENTITY(1,1),
    AvailabilityName NVARCHAR(50) NOT NULL
);

-- Create Movies Table
CREATE TABLE Movies (
    MovieID INT PRIMARY KEY IDENTITY(1,1),
    Title NVARCHAR(255) NOT NULL,
    ReleaseYear INT NOT NULL,
    CategoryID INT NOT NULL,
    IMDBRating FLOAT,
    ImageURL NVARCHAR(255),
    Watched BIT DEFAULT 0,
    WatchedDate DATE,
    FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
);

-- Create Actors Table
CREATE TABLE Actors (
    ActorID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL
);

-- Create MovieActors Table
CREATE TABLE MovieActors (
    MovieID INT NOT NULL,
    ActorID INT NOT NULL,
    PRIMARY KEY (MovieID, ActorID),
    FOREIGN KEY (MovieID) REFERENCES Movies(MovieID),
    FOREIGN KEY (ActorID) REFERENCES Actors(ActorID)
);

-- Create MovieAvailabilities Table
CREATE TABLE MovieAvailabilities (
    MovieID INT NOT NULL,
    AvailabilityID INT NOT NULL,
    PRIMARY KEY (MovieID, AvailabilityID),
    FOREIGN KEY (MovieID) REFERENCES Movies(MovieID),
    FOREIGN KEY (AvailabilityID) REFERENCES Availabilities(AvailabilityID)
);