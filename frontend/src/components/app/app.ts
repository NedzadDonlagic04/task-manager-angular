import { Component, signal } from '@angular/core';
import {MatButtonModule} from '@angular/material/button';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [MatButtonModule],
})
export class App {
  protected readonly title = signal('frontend');

  protected changeTitle = () => this.title() === 'frontend'? this.title.set('backend') : this.title.set('frontend');
}
