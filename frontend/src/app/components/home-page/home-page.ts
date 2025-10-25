import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule, MatIcon } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router } from '@angular/router';

@Component({
    selector: 'app-home-page',
    imports: [
        MatButtonModule,
        MatToolbarModule,
        MatIconModule,
        MatCardModule,
        MatIcon,
    ],
    templateUrl: './home-page.html',
    styleUrl: './home-page.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
})
export class HomePage {
    private _router = inject(Router);

    protected redirectToAddTaskPage() {
        this._router.navigate(['/add-task']);
    }
}
