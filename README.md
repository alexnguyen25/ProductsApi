# Products + Categories API

A simple REST API built with ASP.NET Core Web API, Entity Framework Core, and SQLite. This project is a stripped-down version of a full e-commerce API, scoped to cover the core of WebstaurantStore's tech stack before a SWE internship.

---

## Tech Stack

- **Language:** C#
- **Framework:** ASP.NET Core Web API
- **ORM:** Entity Framework Core
- **Database:** SQLite
- **Testing:** Postman

---

## Database Schema

### Categories
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary key |
| Name | string | |
| IsDeleted | bool | Soft delete flag |

### Products
| Column | Type | Notes |
|---|---|---|
| Id | int | Primary key |
| Name | string | |
| Price | decimal | |
| CategoryId | int | Foreign key → Categories.Id |
| IsDeleted | bool | Soft delete flag |

**Relationship:** One Category → Many Products (one-to-many)

---

## Endpoints

### Products

| Method | Route | Description |
|---|---|---|
| GET | `/products` | Get all products (excludes soft-deleted, paginated) |
| GET | `/products/{id}` | Get one product by ID |
| POST | `/products` | Create a product |
| PUT | `/products/{id}` | Update a product |
| DELETE | `/products/{id}` | Soft delete a product |

### Categories

| Method | Route | Description |
|---|---|---|
| GET | `/categories` | Get all categories (excludes soft-deleted) |
| GET | `/categories/{id}` | Get one category by ID |
| POST | `/categories` | Create a category |
| PUT | `/categories/{id}` | Update a category |
| DELETE | `/categories/{id}` | Soft delete a category |

> **Note:** Deletes are soft deletes — rows are never removed. `IsDeleted` is set to `true` and the record is excluded from list results.

---

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)
- [Postman](https://www.postman.com/) (for testing)

### Setup

```bash
git clone <repo-url>
cd <project-folder>
dotnet restore
dotnet ef database update
dotnet run
```

The API will be available at `https://localhost:<port>` — check your terminal output for the exact port.

---

## Project Structure

```
/Controllers        # Route handlers (ProductsController, CategoriesController)
/Models             # Entity classes (Product, Category)
/Data               # DbContext and EF Core configuration
/Migrations         # Auto-generated EF Core migration files
```

---

## Design Decisions

**Soft deletes** — Records are never hard-deleted. Setting `IsDeleted = true` preserves historical data (important for order history, auditing, and retail integrity).

**Pagination on GET /products** — Prevents returning unbounded result sets; essential at any real scale.

**SQLite** — Chosen for simplicity. The ORM layer (Entity Framework Core) means the same code runs against SQL Server in production with a one-line config change.

---

## 7-Day Build Plan

| Day | Focus |
|---|---|
| 1 | Concepts + environment setup |
| 2 | Schema and endpoint planning |
| 3 | Database setup with Entity Framework Core |
| 4 | Products endpoints |
| 5 | Categories endpoints |
| 6 | Pagination + cleanup |
| 7 | Polish — make sure every line can be explained |
