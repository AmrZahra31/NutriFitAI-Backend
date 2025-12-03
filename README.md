# NutriFitAI-Backend

**AI-powered Personalized Nutrition & Fitness Backend**

This backend API powers a personalized nutrition and fitness recommendation system. It generates **custom meal plans and workouts** based on user data such as age, weight, height, dietary preferences, fitness goals, and budget. Built with **.NET 8**, it follows an **N-tier architecture** with AI microservices, supporting **authentication, authorization**, and **meal history tracking**.

---

## Features

- User registration & login (JWT authentication)
- Personalized meal & workout recommendations
- Budget-aware diet plans
- CRUD operations for meals, workouts, and user data
- History tracking for previously generated plans
- AI integration for personalized recommendations
- Macros tracking and integration
- Flexible account deletion

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- SQL Server or any supported database
- Optional: Postman or similar tool to test API endpoints

### Clone the repository

```bash
git clone https://github.com/AmrZahra31/NutriFitAI-Backend.git
cd NutriFitAI-Backend
```

### Setup

1. Copy example configuration:

```bash
cp appsettings.Development.example.json appsettings.Development.json
```

```bash
cp appsettings.example.json appsettings.json
```

2. Update connection strings, JWT secrets, or API keys in `appsettings.json`.

3. Apply migrations:

```bash
dotnet ef database update
```

### Run the API

```bash
dotnet run --project FitnessApp
```

### API URL

When running locally, the API will be hosted on the ports defined in:

`Properties/launchSettings.json`

Example:
```bash
"applicationUrl": "https://localhost:xxxx;http://localhost:yyyy"
```

Replace the ports with your actual values.


---


## CI/CD with GitHub Actions

- Automated **build**, **test**, **coverage generation**, and **Codecov upload**
- Workflow file: `.github/workflows/dotnet.yml`
- Runs on every push or pull request to `main`

---

## Tech Stack

- **Backend:** ASP.NET Core 8, Entity Framework Core  
- **Database:** SQL Server  
- **CI/CD:** GitHub Actions, Codecov

---

## License

This project is licensed under the [MIT License](LICENSE).
