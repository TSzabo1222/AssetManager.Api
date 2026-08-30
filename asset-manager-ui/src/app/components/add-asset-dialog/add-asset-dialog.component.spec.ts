import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAssetDialogComponent } from './add-asset-dialog.component';

describe('AddAssetDialogComponent', () => {
  let component: AddAssetDialogComponent;
  let fixture: ComponentFixture<AddAssetDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddAssetDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddAssetDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
