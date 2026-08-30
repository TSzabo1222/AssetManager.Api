import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Asset } from '../../services/asset.model';

@Component({
  selector: 'app-add-asset-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './add-asset-dialog.component.html',
  styleUrl: './add-asset-dialog.component.scss'
})
export class AddAssetDialogComponent {
  name = '';
  type = '';
  serialNumber = '';
  purchaseDate: string = new Date().toISOString().substring(0, 10);

  constructor(public dialogRef: MatDialogRef<AddAssetDialogComponent>) {}

  get isValid(): boolean {
    return !!this.name && !!this.type && !!this.serialNumber;
  }

  confirmCreate(): void {
    if (!this.isValid) return;

    const newAsset: Partial<Asset> = {
      name: this.name,
      type: this.type,
      serialNumber: this.serialNumber,
      purchaseDate: this.purchaseDate
    };

    this.dialogRef.close(newAsset);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}