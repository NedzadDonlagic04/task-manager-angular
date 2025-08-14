import { Component, signal } from "@angular/core";

//Amer Just putting in some angular material components to test
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";
import { MatIcon, MatIconModule } from "@angular/material/icon";
import { MatCardModule } from "@angular/material/card";
import { routes } from "./app.routes";
import { RouterLink, RouterOutlet } from "@angular/router";

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
export class App {
    routerLinks = routes;
    isDarkTheme = false;

    toggleDarkTheme() {
        console.log("Toggling dark theme");

        this.isDarkTheme = !this.isDarkTheme;

        if (this.isDarkTheme) {
            document.body.classList.add("dark-theme");
        } else {
            document.body.classList.remove("dark-theme");
        }
    }
}
