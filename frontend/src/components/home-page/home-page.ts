import { ChangeDetectionStrategy, Component } from "@angular/core";
import { MatButtonModule } from "@angular/material/button";
import { MatCardModule } from "@angular/material/card";
import { MatIconModule, MatIcon } from "@angular/material/icon";
import { MatToolbarModule } from "@angular/material/toolbar";
import { RouterLink } from "@angular/router";

@Component({
    selector: "app-home-page",
    imports: [
        MatButtonModule,
        MatToolbarModule,
        MatIconModule,
        MatCardModule,
        MatIcon,
        RouterLink,
    ],
    templateUrl: "./home-page.html",
    styleUrl: "./home-page.css",
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HomePage {}
