import {
    ApplicationConfig,
    provideBrowserGlobalErrorListeners,
    provideZoneChangeDetection,
} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideNativeDateAdapter } from '@angular/material/core';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { httpErrorHandling } from '../../interceptors/httpErrorHandling.interceptor';

export const appConfig: ApplicationConfig = {
    providers: [
        provideBrowserGlobalErrorListeners(),
        provideNativeDateAdapter(),
        provideHttpClient(withInterceptors([httpErrorHandling])),
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes),
    ],
};
