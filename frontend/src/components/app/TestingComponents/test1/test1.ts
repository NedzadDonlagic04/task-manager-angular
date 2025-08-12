import { ChangeDetectionStrategy, Component } from '@angular/core';

@Component({
  selector: 'app-test1',
  imports: [],
  templateUrl: './test1.html',
  styleUrl: './test1.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class Test1 {

}
