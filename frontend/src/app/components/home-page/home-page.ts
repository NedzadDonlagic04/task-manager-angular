import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule, MatIcon } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { Router, RouterLink } from '@angular/router';

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
})
export class HomePage {
    private router = inject(Router);

    protected redirectToAddTaskPage() {
        this.router.navigate(['/add-task']);
    }
}
