import { Component, Inject, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { InventoryItem } from '../../services/asset.model';

@Component({
  selector: 'app-add-inventory-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './add-inventory-dialog.component.html',
  styleUrl: './add-inventory-dialog.component.scss'
})
export class AddInventoryDialogComponent {
  name = '';
  sku = '';
  quantity = 0;
  category = '';
  location = '';
  isEditMode = false;

  constructor(
    public dialogRef: MatDialogRef<AddInventoryDialogComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data: InventoryItem | null
  ) {
    if (data) {
      this.isEditMode = true;
      this.name = data.name;
      this.sku = data.sku;
      this.quantity = data.quantity;
      this.category = data.category;
      this.location = data.location;
    }
  }

  get isValid(): boolean {
    return !!this.name && !!this.sku;
  }

  confirmCreate(): void {
    if (!this.isValid) return;
    const item: Partial<InventoryItem> = {
      name: this.name,
      sku: this.sku,
      quantity: this.quantity,
      category: this.category,
      location: this.location
    };
    this.dialogRef.close(item);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}