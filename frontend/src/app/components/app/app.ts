import { Component, inject, OnInit } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { MatCardModule } from '@angular/material/card';
import { routes } from './app.routes';
import { RouterLink, RouterOutlet } from '@angular/router';
import { StorageService } from '../../services/storage.service';
import { MatMenu, MatMenuTrigger } from '@angular/material/menu';

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
        MatMenu,
        MatMenuTrigger,
    ],
})
export class App implements OnInit {
    protected readonly routes = routes;
    protected isDarkTheme = false;

    private readonly _themeStorageKey = 'isDarkTheme';
    private readonly _storageService = inject(StorageService);

    public ngOnInit(): void {
        const themeStorageValue = this._storageService.getItem<boolean>(
            this._themeStorageKey,
        );

        if (themeStorageValue) {
            this.isDarkTheme = themeStorageValue;
            this._applyCSSForTheme();
        }
    }

    protected toggleDarkTheme(): void {
        this.isDarkTheme = !this.isDarkTheme;
        this._storageService.setItem(this._themeStorageKey, this.isDarkTheme);

        this._applyCSSForTheme();
    }

    private _applyCSSForTheme(): void {
        if (this.isDarkTheme) {
            document.body.classList.add('dark-theme');
        } else {
            document.body.classList.remove('dark-theme');
        }
    }
}
