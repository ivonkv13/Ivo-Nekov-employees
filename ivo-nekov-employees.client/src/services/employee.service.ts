import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { EmployeePair } from '../app/models/employee-pair';

@Injectable({
  providedIn: 'root',
})
export class EmployeeService {
  private apiUrl = 'https://localhost:7288/api/Employee/GetEmployeePairs';

  constructor(private http: HttpClient) {}

  getEmployeePairs$(filename: string): Observable<EmployeePair[]> {
    const url = `${this.apiUrl}?filename=${encodeURIComponent(filename)}`;
    return this.http.get<EmployeePair[]>(url);
}
}
