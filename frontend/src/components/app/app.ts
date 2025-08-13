import { Component, signal } from "@angular/core";

//Amer Just putting in some angular material components to test
import { MatToolbarModule } from "@angular/material/toolbar";
import { MatButtonModule } from "@angular/material/button";
import { MatIcon, MatIconModule } from "@angular/material/icon";
import { MatCardModule } from "@angular/material/card";

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
    ],
})
export class App {
    isDarkTheme = false;

    toggleDarkTheme() {
        console.log("Toggling dark theme");
    }
}
