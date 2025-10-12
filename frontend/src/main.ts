import { bootstrapApplication } from '@angular/platform-browser';
import { appConfig } from './app/components/app/app.config';
import { App } from './app/components/app/app';
/*
    This is either gonna be imported globally
    or in every spot where class-transformer is used

    I opted for the former
*/
import 'reflect-metadata';

bootstrapApplication(App, appConfig).catch((err) => console.error(err));
