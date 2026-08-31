import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { EmployeeService } from '../../services/employee.service';
import { Employee } from '../../services/asset.model';
import { AddEmployeeDialogComponent } from '../add-employee-dialog/add-employee-dialog.component';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatButtonModule, MatIconModule, MatDialogModule],
  templateUrl: './employee-list.component.html',
  styleUrl: './employee-list.component.scss'
})
export class EmployeeListComponent implements OnInit {
  employees: Employee[] = [];
  displayedColumns: string[] = ['fullName', 'email', 'department', 'position', 'hireDate'];

  constructor(private employeeService: EmployeeService, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.employeeService.getAll().subscribe({
      next: (data) => (this.employees = data),
      error: (err) => console.error('Error loading employees:', err)
    });
  }

  openAddDialog(): void {
    const ref = this.dialog.open(AddEmployeeDialogComponent);
    ref.afterClosed().subscribe((newEmployee) => {
      if (!newEmployee) return;
      this.employeeService.create(newEmployee).subscribe({
        next: () => this.load(),
        error: (err) => console.error('Error creating employee:', err)
      });
    });
  }
}