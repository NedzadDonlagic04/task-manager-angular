import { Component, OnInit } from "@angular/core";
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";
import { MatIcon, MatIconModule } from "@angular/material/icon";
import { MatCardModule } from "@angular/material/card";
import { routes } from "./app.routes";
import { RouterLink, RouterOutlet } from "@angular/router";
import { StorageService } from "../../services/storage.service";

@Component({
    selector: "app-root",
    standalone: true,
    templateUrl: "./app.html",
    styleUrls: ["./app.css"],
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
    readonly themeStorageKey = "isDarkTheme";

    routerLinks = routes;
    isDarkTheme = false;

    constructor(private storageService: StorageService) {}

    ngOnInit(): void {
        const themeStorageValue = this.storageService.getItem<boolean>(
            this.themeStorageKey,
        );

        if (themeStorageValue) {
            this.isDarkTheme = themeStorageValue;
            this.applyCSSForTheme();
        }
    }

    toggleDarkTheme(): void {
        this.isDarkTheme = !this.isDarkTheme;
        this.storageService.setItem(this.themeStorageKey, this.isDarkTheme);

        this.applyCSSForTheme();
    }

    applyCSSForTheme(): void {
        if (this.isDarkTheme) {
            document.body.classList.add("dark-theme");
        } else {
            document.body.classList.remove("dark-theme");
        }
    }
}
