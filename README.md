# CQR - A Simple CQRS Library for .NET Core

CQR is a lightweight, open-source library that provides a simple alternative to MediatR for implementing **CQRS (Command-Query Responsibility Segregation)** in .NET applications.  
It focuses exclusively on handling **commands and queries** with asynchronous support, allowing developers to decouple request handling logic from the calling code.

> Note: This library is intentionally minimal and does **not** include advanced MediatR features such as notifications, pipelines, or behaviors. It is designed purely for CQRS.

---

## Features

- Send commands and queries with or without a response.
- Automatic handler resolution with dependency injection support.
- Simple, easy-to-understand API.
- Supports both `IRequest<TResponse>` and `IRequest` (fire-and-forget) patterns.
- Internal caching for efficient handler lookup.

---

## Installation

You can install CQR via NuGet:

```bash
dotnet add package CQR --version 1.0.0
```

Or using the NuGet Package Manager:

```bash
Install-Package CQR -Version 1.0.0
```

## Getting Started (Example)

### Step 01:
Add CQR to DI Services

```c#
builder.Services.AddCQR(config =>
{
    config.AddAssembly(typeof(Program).Assembly);
    ...
});
```
You can add assemblies as many as you want.

### Step 02:
Create Commands and Queries

#### A command class:
```c#
using CQR.Abstractions;

namespace Application.Commands;

public class CreateTaskCommand : IRequest
{
    public string AssignedUser { get; set; }
    public string Name {get;set;}
    public string Description {get;set;}
}
```
In here, ``CreateTaskCommand`` implements `IRequest` from the `CQR.Abstractions` package. 

#### A query class:
```c#
using CQR.Abstractions;
using Domain.Models;

namespace Application.Queries.GetAllTasksQuery;

public class GetAllTaskQuery : IRequest<TaskModel[]>
{
    
}
```

In here, `GetAllTaskQuery` implements `IRequest<TaskModel[]>`, specifying the expected result type.

### Step 03
Create command and query handlers

Handlers are responsible for processing commands and queries. Each command or query class must have a corresponding handler that contains the business logic for that operation.

#### A command handler:
```c#
using CQR.Abstractions;
using Infrastructure.Repository;

namespace Application.Commands;

public class CreateTaskCommandHandler(ITaskRepository repository) : IRequestHandler<CreateTaskCommand>
{
    public async Task HandleRequestAsync(CreateTaskCommand request)
    {
        await repository.AddTask(request);
    }
}
```

#### A query handler:
```c#
using CQR.Abstractions;
using Domain.Models;
using Infrastructure.Repository;

namespace Application.Queries.GetAllTasksQuery;

public class GetAllTaskQueryHandler : IRequestHandler<GetAllTaskQuery, TaskModel[]>
{
    
    private readonly ITaskRepository _repository;

    public GetAllTaskQueryHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskModel[]> HandleRequestAsync(GetAllTaskQuery request)
    {
        var res = await _repository.GetAllTasks();
        return res.ToArray();
    }
}
```

### Step 04
In this final step, we integrate the CQRS-based commands and queries into an actual API controller using the `ICqr` abstraction. This provides a clean and centralized way to dispatch commands and queries without tightly coupling the controller to specific implementations.

```c#
using Application.Commands;
using Application.Commands.DeleteTaskCommand;
using Application.Queries.GetAllTasksQuery;
using Application.Queries.GetTaskByUserQuery;
using CQR.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlow.CQRS.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaskController: ControllerBase
{
    private readonly ICqr _cqr;

    public TaskController(ICqr cqr)
    {
        _cqr = cqr;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskCommand request)
    {
        await _cqr.SendRequest(request);
        return Ok("Created a Task");
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks()
    {
        var result = await _cqr.SendRequest(new GetAllTaskQuery());
        return Ok(result);
    }
}
```

✅ With this step, your CQRS-based API is now fully functional — cleanly separated into commands, queries, handlers, and endpoints.

## Questions or Feedback?

If you have any questions or feedback about this implementation or the CQRS pattern in general, don't hesitate to reach out or open a discussion.

Thanks for checking out this project! 🎉

**Happy coding! 🚀**