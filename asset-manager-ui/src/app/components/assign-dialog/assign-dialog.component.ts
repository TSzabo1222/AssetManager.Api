import { Component, Inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { Asset, Employee, AssetStatus } from '../../services/asset.model';
import { EmployeeService } from '../../services/employee.service';

export interface AssignDialogData {
  asset: Asset;
}

@Component({
  selector: 'app-assign-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule, MatDialogModule, MatFormFieldModule, MatSelectModule, MatButtonModule],
  templateUrl: './assign-dialog.component.html',
  styleUrl: './assign-dialog.component.scss'
})
export class AssignDialogComponent implements OnInit {
  AssetStatus = AssetStatus;
  employees: Employee[] = [];
  selectedEmployeeId: number | null = null;

  constructor(
    public dialogRef: MatDialogRef<AssignDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: AssignDialogData,
    private employeeService: EmployeeService
  ) {}

  ngOnInit(): void {
    this.employeeService.getAll().subscribe({
      next: (data) => (this.employees = data),
      error: (err) => console.error('Error loading employees:', err)
    });
  }

  confirmAssign(): void {
    if (this.selectedEmployeeId) {
      this.dialogRef.close({ action: 'assign', employeeId: this.selectedEmployeeId });
    }
  }

  confirmReturn(): void {
    this.dialogRef.close({ action: 'return' });
  }

  cancel(): void {
    this.dialogRef.close();
  }
}