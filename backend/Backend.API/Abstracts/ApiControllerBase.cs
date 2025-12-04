using System.Diagnostics;
using System.Net;
using Backend.API.Extensions;
using Backend.Shared.Records;
using Microsoft.AspNetCore.Mvc;

namespace Backend.API.Abstracts;

[ApiController]
public abstract class ApiControllerBase : ControllerBase
{
    protected StatusCodeResult ResetContent() => StatusCode(StatusCodes.Status205ResetContent);

    protected ObjectResult Problem(Error error)
    {
        var problemDetails = CreateProblemDetails(error);

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }

    protected ObjectResult Problem(List<Error> errors)
    {
        ArgumentNullException.ThrowIfNull(errors, nameof(errors));
        ArgumentOutOfRangeException.ThrowIfZero(errors.Count, nameof(errors));

        var error = errors.First();
        var problemDetails = CreateProblemDetails(error);

        if (errors.Count > 1)
        {
            problemDetails.Extensions["additionalErrors"] = errors
                .Skip(1)
                .Select(error => new { title = error.Code, detail = error.Detail });
        }

        return new ObjectResult(problemDetails) { StatusCode = problemDetails.Status };
    }

    protected IPAddress? ClientIPAddress => HttpContext.Connection.RemoteIpAddress;

    private ProblemDetails CreateProblemDetails(Error error)
    {
        ArgumentNullException.ThrowIfNull(error, nameof(error));

        int statusCode = error.Type.GetStatusCode();
        string traceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

        var problemDetails = new ProblemDetails
        {
            Type = $"https://httpstatuses.io/{statusCode}",
            Title = error.Code,
            Detail = error.Detail,
            Status = statusCode,
            Instance = HttpContext.Request.Path,
        };

        problemDetails.Extensions["traceId"] = traceId;

        return problemDetails;
    }
}
