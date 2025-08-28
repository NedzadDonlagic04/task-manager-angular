import {
  ChangeDetectionStrategy,
  Component,
  EventEmitter,
  Input,
  Output,
} from '@angular/core';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-custom-button',
  imports: [MatIcon],
  templateUrl: './custom-button.html',
  styleUrl: './custom-button.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CustomButton {
  @Input() buttonText: string = 'Button';
  @Input() buttonMatIcon: string = 'favorite';
  @Input() isButtonDisabled: boolean = false;

  @Output() buttonClick = new EventEmitter<void>();

  onClick(): void {
    this.buttonClick.emit();
  }

  toggleButtonState(): void {
    this.isButtonDisabled = !this.isButtonDisabled;
  }
}
