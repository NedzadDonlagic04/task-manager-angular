import { ComponentFixture, TestBed } from '@angular/core/testing';

import { YesNoDialog } from './yes-no-dialog';

describe('YesNoDialog', () => {
  let component: YesNoDialog;
  let fixture: ComponentFixture<YesNoDialog>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [YesNoDialog]
    })
    .compileComponents();

    fixture = TestBed.createComponent(YesNoDialog);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
