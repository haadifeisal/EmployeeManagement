# EmployeeManagement

Scaffold:
Scaffold-DbContext "Server=(LocalDB)\MSSQLLocalDB;Database=EmployeeManagement;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Repository\EmployeeManagement -f

dotnet ef migrations add InitialCreate

dotnet ef database update
