import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddInventoryDialogComponent } from './add-inventory-dialog.component';

describe('AddInventoryDialogComponent', () => {
  let component: AddInventoryDialogComponent;
  let fixture: ComponentFixture<AddInventoryDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddInventoryDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddInventoryDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
