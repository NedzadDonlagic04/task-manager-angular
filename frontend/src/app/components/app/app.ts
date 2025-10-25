import { Component, inject } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { AppRoute, routes } from './app.routes';
import { RouterLink, RouterOutlet } from '@angular/router';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';
import { ThemeService } from '../../services/theme.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.html',
    styleUrl: './app.scss',
    imports: [
        MatButtonModule,
        MatToolbarModule,
        MatIconModule,
        MatCardModule,
        MatIcon,
        RouterLink,
        RouterOutlet,
        MatMenu,
        MatMenuTrigger,
    ],
    standalone: true,
})
export class App {
    protected readonly navRoutes: AppRoute[] = routes.filter(
        (route: AppRoute) => route.showInNav,
    );

    protected readonly themeService = inject(ThemeService);

    protected toggleDarkTheme(): void {
        this.themeService.toggleDarkTheme();
    }
}
