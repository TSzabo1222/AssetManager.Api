import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from './asset.model';

const API_URL = 'https://localhost:65503/api/Employees';

@Injectable({ providedIn: 'root' })
export class EmployeeService {
  constructor(private http: HttpClient) {}

  getAll(): Observable<Employee[]> {
    return this.http.get<Employee[]>(API_URL);
  }

  create(employee: Partial<Employee>): Observable<Employee> {
    return this.http.post<Employee>(API_URL, employee);
  }
}