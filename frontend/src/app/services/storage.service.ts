import { Injectable } from "@angular/core";

@Injectable({
    providedIn: "root",
})
export class StorageService {
    setItem(key: string, item: any): void {
        try {
            localStorage.setItem(key, JSON.stringify(item));
        } catch (error) {
            console.error(`Error while saving item -> ${error}`);
        }
    }

    getItem<T>(key: string): T | null {
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

    removeItem(key: string): void {
        try {
            localStorage.removeItem(key);
        } catch (error) {
            console.error(`Error while removing item -> ${error}`);
        }
    }

    clearItems(): void {
        try {
            localStorage.clear();
        } catch (error) {
            console.error(`Error while clearing items -> ${error}`);
        }
    }
}
