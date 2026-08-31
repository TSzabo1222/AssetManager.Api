import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Employee } from '../../services/asset.model';

@Component({
  selector: 'app-add-employee-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule, MatFormFieldModule, MatInputModule, MatButtonModule],
  templateUrl: './add-employee-dialog.component.html',
  styleUrl: './add-employee-dialog.component.scss'
})
export class AddEmployeeDialogComponent {
  fullName = '';
  email = '';
  department = '';
  position = '';
  hireDate: string = new Date().toISOString().substring(0, 10);

  constructor(public dialogRef: MatDialogRef<AddEmployeeDialogComponent>) {}

  get isValid(): boolean {
    return !!this.fullName && !!this.email;
  }

  confirmCreate(): void {
    if (!this.isValid) return;
    const newEmployee: Partial<Employee> = {
      fullName: this.fullName,
      email: this.email,
      department: this.department,
      position: this.position,
      hireDate: this.hireDate
    };
    this.dialogRef.close(newEmployee);
  }

  cancel(): void {
    this.dialogRef.close();
  }
}