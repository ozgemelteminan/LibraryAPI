# 🐝 BookHive

A **RESTful API** built with **ASP.NET Core 9** and **Entity Framework Core** for managing a simple library system.  

It exposes endpoints to manage **students**, **libraries**, **books**, and **borrowing/returning** flow, and comes with built-in **Swagger UI** so you can test endpoints directly in the browser.

> ℹ️ **Auth**: JWT authentication is configured in the pipeline, but currently **no endpoints are protected** with `[Authorize]`. You can add `[Authorize]` attributes on controllers/actions when you’re ready.

<br>

## ⚡ Features

- Students: `register`, `login (JWT token)`, list students, get by id. 
- Libraries: CRUD (create, list, get, update).  
- Books: create, list, update (includes `Year` and `ISBN`).  
- Borrow/Return: `borrow` a book, `return` a book, view student borrowing history.  
- Reports: list books by student or by library.  
- Swagger UI & OpenAPI.

<br>

## 🚀 Getting Started with Prerequisites

Make sure you have installed:
> [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)  

⚠️ **Note about SQL Server**  
>This project is already connected to a remote SQL Server.  
If you have access to that server, you **do not need to install SQL Server locally**.  

👉 Correct connection string in `appsettings.json` (already configured ✅)  

<br>

## 🛠️ Testing & Installation

 ### 📥 Clone or download the project:  
   ```bash
   git clone https://github.com/ozgemelteminan/bookhive-backend
   ```

 ### 📂 Navigate into the project folder:
   ```bash
   cd LibraryApi
   ```

 ### 🔧 Restore dependencies
   ```bash
   dotnet restore
   ```

 ### ▶️ Run the Project
   ```bash
   dotnet run
   ```

 ### 🌐 Open Swagger
   ```bash
   https://localhost:5274/swagger
   ```
<br>

## 🦾 Tech Stack

- **Target Framework:** .NET **9.0** (`net9.0`)  
- **Packages:**  
  - `Microsoft.EntityFrameworkCore.SqlServer` 9.0.8  
  - `Microsoft.EntityFrameworkCore.Tools` 9.0.8  
  - `Microsoft.AspNetCore.Authentication.JwtBearer` 9.0.8  
  - `Swashbuckle.AspNetCore` 9.0.3  
- **Database:** SQL Server (see connection string in *appsettings.json*)  
- **CORS:** Policy **AllowAll** (any origin/method/header)

<br>

## 📂 Project Structure

```
LibraryApi/
├─ Controllers/
│  ├─ BooksController.cs
│  ├─ LibrariesController.cs
│  ├─ ReportsController.cs
│  ├─ StudentBooksController.cs
│  └─ StudentsController.cs
├─ Data/
│  └─ LibraryContext.cs
├─ Migrations/
├─ Models/
│  ├─ DTOs/ (BookDto, LibraryDto, StudentBookDto, StudentLoginDto)
│  ├─ Book.cs, Library.cs, Student.cs, StudentBook.cs, StudentBookHistory.cs
├─ Program.cs
├─ appsettings.json
├─ appsettings.Development.json
└─ LibraryApi.csproj
```

<br>

## 🔗 API Endpoints

> Base route convention: `[Route("api/[controller]")]` → controller name defines the segment (e.g., `StudentsController` → `/api/Students`).

### Students (`/api/Students`)
| Method | Path                 | Description                     |
|-------:|----------------------|---------------------------------|
| GET    | `/api/Students`      | List all students               |
| GET    | `/api/Students/{id}` | Get a student by id             |
| POST   | `/api/Students/register` | Register a new student     |
| POST   | `/api/Students/login`    | Login and receive JWT token |


**Register – Request example**
```http
POST /api/Students/register
Content-Type: application/json

{
  "fullName": "Ada Lovelace",
  "email": "ada@example.com",
  "password": "P@ssw0rd!"
}
```

**Login – Request & Response example**
```http
POST /api/Students/login
Content-Type: application/json

{
  "email": "ada@example.com",
  "password": "P@ssw0rd!"
}
```
```json
{
  "id": 1,
  "fullName": "Ada Lovelace",
  "email": "ada@example.com",
  "token": "<JWT_TOKEN>"
}
```


### Libraries (`/api/Libraries`)
| Method | Path                     | Description          |
|-------:|--------------------------|----------------------|
| GET    | `/api/Libraries`         | List libraries       |
| GET    | `/api/Libraries/{id}`    | Get library by id    |
| POST   | `/api/Libraries`         | Create library       |
| PUT    | `/api/Libraries/{id}`    | Update library       |


**Create Library – Request example**
```http
POST /api/Libraries
Content-Type: application/json

{
  "name": "Central Library",
  "location": "Baker Street 221B"
}
```

**Update Library – Request example**
```http
PUT /api/Libraries/3
Content-Type: application/json

{
  "id": 3,
  "name": "Central Library - New Wing",
  "location": "Baker Street 221B"
}
```


### Books (`/api/Books`)
| Method | Path               | Description   |
|-------:|--------------------|---------------|
| GET    | `/api/Books`       | List books    |
| POST   | `/api/Books`       | Create book   |
| PUT    | `/api/Books/{id}`  | Update book   |

> Note: `BookDto` uses `Isbn` (lowercase `bn`) while the entity property is `ISBN`. Both `Year` and `Isbn` are required when creating/updating.

**Create Book – Request example**
```http
POST /api/Books
Content-Type: application/json

{
  "title": "Clean Code",
  "author": "Robert C. Martin",
  "libraryId": 1,
  "year": 2008,
  "isbn": "9780132350884"
}
```

**Update Book – Request example**
```http
PUT /api/Books/5
Content-Type: application/json

{
  "id": 5,
  "title": "Clean Code (2nd Edition)",
  "author": "Robert C. Martin",
  "libraryId": 1,
  "year": 2024,
  "isbn": "9780132350884"
}
```


### Borrowing (`/api/StudentBooks`)
| Method | Path                                        | Description                                  |
|-------:|---------------------------------------------|----------------------------------------------|
| GET    | `/api/StudentBooks`                         | List **active** borrowed books               |
| POST   | `/api/StudentBooks`                         | Borrow a book                                |
| DELETE | `/api/StudentBooks/{studentId}/{bookId}`    | **Return** a book (moves record to history)  |
| GET    | `/api/StudentBooks/history/{studentId}`     | Borrow/return history for a student          |


**Borrow – Request example**
```http
POST /api/StudentBooks
Content-Type: application/json

{
  "studentId": 1,
  "bookId": 5
}
```
> Validation: A book cannot be borrowed by more than one student at the same time; the same student cannot borrow the same book again before returning it.

**Return – Request example**
```http
DELETE /api/StudentBooks/1/5
```


### Reports (`/api/Reports`)
| Method | Path                               | Description                     |
|-------:|------------------------------------|---------------------------------|
| GET    | `/api/Reports/student/{studentId}` | Books currently borrowed by student |
| GET    | `/api/Reports/library/{libraryId}` | Books belonging to a library     |

<br>

## 🔒 Authentication (JWT)

- JWT authentication is configured in appsettings.json under the `Jwt` section, including `Key`, `Issuer`, and `Audience`.  
- Middleware is wired in `Program.cs`:
  
  >`UseAuthentication()`
  >` UseAuthorization()`
  
- To **protect** endpoints, decorate controllers/actions with `[Authorize]` and optionally roles/policies.

> Example:
> ```csharp
> [Authorize]
> [HttpGet]
> public IActionResult GetBooks() { ... }
> ```

To call protected endpoints from Swagger, click **Authorize** and paste `Bearer <token>` (from `/api/Students/login`).

<br>

## 🌐 CORS (Cross-Origin Resource Sharing)**

CORS is enabled for all origins, headers, and methods in `Program.cs`:
 > ✅ This allows your API to be accessed from any frontend or external domain.
>
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});  
```
<br>

## License

This project is for educational/demo purposes.
