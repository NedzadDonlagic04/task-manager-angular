import { Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root',
})
export class StorageService {
    public setItem(key: string, item: unknown): void {
        try {
            localStorage.setItem(key, JSON.stringify(item));
        } catch (error) {
            console.error(`Error while saving item -> ${error}`);
        }
    }

    public getItem<T>(key: string): T | null {
        try {
            const item = localStorage.getItem(key);
            if (item) {
                return JSON.parse(item) as T;
            }
            return null;
        } catch (error) {
            console.error(`Error while getting item -> ${error}`);
            return null;
        }
    }

    public removeItem(key: string): void {
        try {
            localStorage.removeItem(key);
        } catch (error) {
            console.error(`Error while removing item -> ${error}`);
        }
    }

    public clearItems(): void {
        try {
            localStorage.clear();
        } catch (error) {
            console.error(`Error while clearing items -> ${error}`);
        }
    }
}
