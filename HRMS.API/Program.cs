using HRMS.Application.CommandHandlers.Contingent;
using HRMS.Application.CommandHandlers.Employee;
using HRMS.Application.CommandHandlers.Employee.EmployeeAddress;
using HRMS.Application.CommandHandlers.JobTitle;
using HRMS.Application.CommandHandlers.Leave;
using HRMS.Application.Commands.Employee;
using HRMS.Application.Queries.Employee;
using HRMS.Application.QueryHandlers.Contingent;
using HRMS.Application.QueryHandlers.Employee;
using HRMS.Application.QueryHandlers.JobTitle;
using HRMS.Application.QueryHandlers.Leave;
using HRMS.Domain.Entities;
using HRMS.Domain.Entities.Payroll;
using HRMS.Domain.Entities.PayslipHistory;
using HRMS.Domain.Entities.Timesheet;
using HRMS.Infrastructure.Interfaces;
using HRMS.Infrastructure.Interfaces.Attendance;
using HRMS.Infrastructure.Interfaces.Company;
using HRMS.Infrastructure.Interfaces.Contingent;
using HRMS.Infrastructure.Interfaces.Departments;
using HRMS.Infrastructure.Interfaces.Designations;
using HRMS.Infrastructure.Interfaces.Divisions;
using HRMS.Infrastructure.Interfaces.Employee;
using HRMS.Infrastructure.Interfaces.HelpDesk;
using HRMS.Infrastructure.Interfaces.JobTitle;
using HRMS.Infrastructure.Interfaces.Leave;
using HRMS.Infrastructure.Interfaces.Location;
using HRMS.Infrastructure.Interfaces.Payroll;
using HRMS.Infrastructure.Interfaces.PayrollDeduction;
using HRMS.Infrastructure.Interfaces.PayrollWages;
using HRMS.Infrastructure.Interfaces.PayslipHistory;
using HRMS.Infrastructure.Interfaces.ProjectAssigment;
using HRMS.Infrastructure.Interfaces.Projects;
using HRMS.Infrastructure.Interfaces.RolePermission;
using HRMS.Infrastructure.Interfaces.Roles;
using HRMS.Infrastructure.Interfaces.Timesheet;
using HRMS.Infrastructure.Interfaces.User;
using HRMS.Infrastructure.Repositories;
using HRMS.Infrastructure.Repositories.Attendance;
using HRMS.Infrastructure.Repositories.Company;
using HRMS.Infrastructure.Repositories.Contingent;
using HRMS.Infrastructure.Repositories.Departments;
using HRMS.Infrastructure.Repositories.Designations;
using HRMS.Infrastructure.Repositories.Divisions;
using HRMS.Infrastructure.Repositories.Employee;
using HRMS.Infrastructure.Repositories.HelpDesk;
using HRMS.Infrastructure.Repositories.JobTitle;
using HRMS.Infrastructure.Repositories.Location;
using HRMS.Infrastructure.Repositories.Payroll;
using HRMS.Infrastructure.Repositories.PayrollDeduction;
using HRMS.Infrastructure.Repositories.PayrollWages;
using HRMS.Infrastructure.Repositories.PayslipHistory;
using HRMS.Infrastructure.Repositories.ProjectAssigment;
using HRMS.Infrastructure.Repositories.Projects;
using HRMS.Infrastructure.Repositories.RolePermission;
using HRMS.Infrastructure.Repositories.Roles;
using HRMS.Infrastructure.Repositories.Timesheet;
using HRMS.Infrastructure.Repositories.User;
using Npgsql;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Retrieve connection string from appsettings.json or environment
var connStr = builder.Configuration.GetConnectionString("EmployeeDb");

//enum declarations
NpgsqlConnection.GlobalTypeMapper.MapEnum<Gender>("gender_enum");
NpgsqlConnection.GlobalTypeMapper.MapEnum<MaritalStatus>("marital_status_enum");
NpgsqlConnection.GlobalTypeMapper.MapEnum<EmployeeStatus>("employment_status_enum");
NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountStatus>("account_status_enum");
NpgsqlConnection.GlobalTypeMapper.MapEnum<InvitationStatus>("invitation_status_enum");
NpgsqlConnection.GlobalTypeMapper.MapEnum<TimeEntryStatus>("time_entry_status_enum");
NpgsqlConnection.GlobalTypeMapper.MapEnum<PayrollRunStatusEnum>("payroll_run_status_enum");
NpgsqlConnection.GlobalTypeMapper.MapEnum<PaymentStatusEnum>("payment_status_enum");


// Register scoped repository for dependency injection
builder.Services.AddScoped<IEmployeeRepository>(sp => new EmployeeRepository(connStr));
builder.Services.AddScoped<IOrganizationRepository>(sp => new OrganizationRepository(connStr));
builder.Services.AddScoped<IDivisionRepository>(sp => new DivisionRepository(connStr));
builder.Services.AddScoped<ICompanyRepository>(sp => new CompanyRepository(connStr));
builder.Services.AddScoped<IDepartmentRepository>(sp => new DepartmentRepository(connStr));
builder.Services.AddScoped<IDesignationRepository>(sp => new DesignationRepository(connStr));
builder.Services.AddScoped<IEmployeeContactsRepository>(sp => new EmployeeContactsRepository(connStr));
builder.Services.AddScoped<IEmployeeFamilyMembersRepository>(sp => new EmployeeFamilyMembersRepository(connStr));
builder.Services.AddScoped<IEmployeeBankAccountsRepository>(sp => new EmployeeBankAccountsRepository(connStr));
builder.Services.AddScoped<IEmployeeAddressesRepository>(sp => new EmployeeAddressesRepository(connStr));
builder.Services.AddScoped<IEmployeeProjectsRepository>(sp => new EmployeeProjectsRepository(connStr));
builder.Services.AddScoped<IJobTitleRepository>(sp => new JobTitleRepository(connStr));
builder.Services.AddScoped<IContingentRepository>(sp => new ContingentRepository(connStr));
builder.Services.AddScoped<IProjectRepository>(sp => new ProjectRepository(connStr));
builder.Services.AddScoped<IProjectAssignmentRepository>(sp => new ProjectAssignmentRepository(connStr));
builder.Services.AddScoped<ILocationRepository>(sp => new LocationRepository(connStr));
builder.Services.AddScoped<ILeaveTypesRepository>(sp => new LeaveTypesRepository(connStr));
builder.Services.AddScoped<IAttendanceRepository>(sp => new AttendanceRepository(connStr));
builder.Services.AddScoped<ITimesheetRepository>(sp => new TimesheetRepository(connStr));
builder.Services.AddScoped<IUserRepository>(sp => new UserRepository(connStr));
builder.Services.AddScoped<IRoleRepository>(sp => new RoleRepository(connStr));
builder.Services.AddScoped<IRolePermissionRepository>(sp => new RolePermissionRepository(connStr));
builder.Services.AddScoped<IPayrollRepository>(sp => new PayrollRepository(connStr));
builder.Services.AddScoped<IPayrollWagesRepository>(sp => new PayrollWagesRepository(connStr));
builder.Services.AddScoped<IPayrollDeductionRepository>(sp => new PayrollDeductionRepository(connStr));
builder.Services.AddScoped<IPayslipHistoryRepository>(sp => new PayslipHistoryRepository(connStr));
builder.Services.AddScoped<IHelpdeskTicketRepository>(sp => new HelpdeskTicketRepository(connStr));




// Register query handlers for organization
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetOrganizationByIdHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateOrganizationHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateOrganizationHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteOrganizationHandler).Assembly));

//// Register command handlers for divisions
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetDivisionByIdQueryHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllDivisionByIdQueryHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDivisionCommandHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateDivisionCommandHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteDivisionCommandHandler ).Assembly));

//// Register command handlers for company
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCompanyByIdQueryHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCompaniesByOrgIdQueryHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCompaniesByDivIdQueryHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCompanyCommandHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateCompanyCommandHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteCompanyCommandHandler).Assembly));


// Register command handlers for department
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDepartmentCommandHandler).Assembly));
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetDepartmentByIdQueryHandler).Assembly));

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateDesignationCommand).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetEmployeeByIdQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllEmployeesByCompanyQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllEmployeesByDivisionQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllEmployeesByOrgnizationQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateEmployeeCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteEmployeeCommandHandler).Assembly));


// Register command handlers for JobTitle
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateJobTitleCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateJobTitleCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetJobTitleByIdQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteJobTitleCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetJobTitleByCompanyIdQueryHandler).Assembly));

// Register command handlers for Contingent
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateContingentCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateContingentCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteContingentCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetContingentByIdQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllContingentByOrgIdQueryHandler).Assembly));

//register command /query handlers for EmployeeContacts
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateEmployeeAddressesCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetEmployeesByFamilyMembersQueryHandler).Assembly));

// Register command handlers for Contingent
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateLeaveTypesCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(UpdateLeaveTypesCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DeleteLeaveTypesCommandHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetLeaveTypesQueryHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllLeaveTypesQueryHandler).Assembly));





// Register controllers and Swagger 
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development    
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();