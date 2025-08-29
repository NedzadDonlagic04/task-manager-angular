import { Component, inject, OnInit } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { routes } from './app.routes';
import { RouterLink, RouterOutlet } from '@angular/router';
import { StorageService } from '../../services/storage.service';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.html',
    styleUrls: ['./app.scss'],
    imports: [
        MatButtonModule,
        MatToolbarModule,
        MatIconModule,
        MatCardModule,
        MatIcon,
        RouterLink,
        RouterOutlet,
    ],
})
export class App implements OnInit {
    private readonly themeStorageKey = 'isDarkTheme';

    protected routerLinks = routes;
    protected isDarkTheme = false;

    private storageService = inject(StorageService);

    public ngOnInit(): void {
        const themeStorageValue = this.storageService.getItem<boolean>(
            this.themeStorageKey,
        );

        if (themeStorageValue) {
            this.isDarkTheme = themeStorageValue;
            this.applyCSSForTheme();
        }
    }

    protected toggleDarkTheme(): void {
        this.isDarkTheme = !this.isDarkTheme;
        this.storageService.setItem(this.themeStorageKey, this.isDarkTheme);

        this.applyCSSForTheme();
    }

    private applyCSSForTheme(): void {
        if (this.isDarkTheme) {
            document.body.classList.add('dark-theme');
        } else {
            document.body.classList.remove('dark-theme');
        }
    }
}
