import {
    DOCUMENT,
    inject,
    Injectable,
    Renderer2,
    RendererFactory2,
} from '@angular/core';
import { StorageService } from './storage.service';

@Injectable({
    providedIn: 'root',
})
export class ThemeService {
    private _isDarkTheme: boolean = false;
    private readonly _themeStorageKey = 'isDarkTheme';
    private readonly _storageService = inject(StorageService);
    private readonly _document = inject(DOCUMENT);
    private readonly _renderer: Renderer2 = inject(
        RendererFactory2,
    ).createRenderer(null, null);

    constructor() {
        this._initializeTheme();
    }

    public isDarkTheme(): boolean {
        return this._isDarkTheme;
    }

    public toggleDarkTheme(): void {
        this._isDarkTheme = !this._isDarkTheme;
        this._saveThemeInStorage();
        this._applyCurrentTheme();
    }

    private _initializeTheme(): void {
        const isDarkTheme = this._storageService.getItem<boolean>(
            this._themeStorageKey,
        );

        if (isDarkTheme === null) {
            this._saveThemeInStorage();
        } else {
            this._isDarkTheme = isDarkTheme;
        }

        this._applyCurrentTheme();
    }

    private _saveThemeInStorage(): void {
        this._storageService.setItem(this._themeStorageKey, this._isDarkTheme);
    }

    private _applyCurrentTheme(): void {
        if (this._isDarkTheme) {
            this._renderer.addClass(this._document.body, 'dark-theme');
        } else {
            this._renderer.removeClass(this._document.body, 'dark-theme');
        }
    }
}
