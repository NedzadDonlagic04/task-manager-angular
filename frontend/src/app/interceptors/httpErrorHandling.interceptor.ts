import {
    HttpErrorResponse,
    HttpEvent,
    HttpHandlerFn,
    HttpRequest,
    HttpStatusCode,
} from '@angular/common/http';
import { inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { catchError, Observable, throwError } from 'rxjs';

function getErrorMessageForHttpStatusCode(statusCode: HttpStatusCode): string {
    if ((statusCode as number) === 0) {
        return 'Network error. Please check your connection.';
    }

    switch (statusCode) {
        case HttpStatusCode.NotFound:
            return "The resource wasn't found.";
        case HttpStatusCode.InternalServerError:
            return 'Server error. Check again later.';
        case HttpStatusCode.Unauthorized:
            return 'Unathorized access. Please login.';
        case HttpStatusCode.Forbidden:
            return 'Access defnied.';
        case HttpStatusCode.BadRequest:
            return 'Bad request. Check your inputs.';
        default:
            return `Request failed with ${statusCode} status.`;
    }
}

export function httpErrorHandling(
    req: HttpRequest<unknown>,
    next: HttpHandlerFn,
): Observable<HttpEvent<unknown>> {
    const matSnackBar = inject(MatSnackBar);

    return next(req).pipe(
        catchError((error: unknown) => {
            let errorMessage = 'Unexpected error occurred.';

            if (error instanceof HttpErrorResponse) {
                errorMessage = getErrorMessageForHttpStatusCode(error.status);
            } else if (error instanceof ProgressEvent) {
                errorMessage = 'Network error. Please check your connection.';
            }

            matSnackBar.open(errorMessage, 'Close', {
                duration: 5000,
                panelClass: ['error-snackbar'],
            });

            return throwError(() => error);
        }),
    );
}
