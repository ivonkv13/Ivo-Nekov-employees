import { Component } from '@angular/core';
import { EmployeePair } from '../../models/employee-pair';
import { EmployeeService } from '../../../services/employee.service';
import { Observable } from 'rxjs';
import { FileService } from '../../../services/file.service';

@Component({
  selector: 'app-employee-table',
  templateUrl: './employee-table.component.html',
  styleUrl: './employee-table.component.css'
})
export class EmployeeTableComponent {
  employeePairs$: Observable<EmployeePair[]> | null = null;
  isLoading = false;
  hasRecords = false;

  files: string[] = [];
  selectedFile: string = '';

  constructor(private employeeService: EmployeeService, private fileService: FileService) {
    this.loadFiles();
  }

  loadFiles(): void {
    this.fileService.getAllFiles().subscribe({
      next: (response) => {
        if (Array.isArray(response)) {
          this.files = response; // ✅ Assign extracted array directly
        } else {
          console.error('Invalid API response:', response);
          this.files = []; // ✅ Prevents UI errors
        }
      },
      error: (error) => {
        console.error('Error fetching files:', error);
        this.files = [];
      }
    });
  }

  onFileSelected(file: string): void {
    this.selectedFile = file;
    console.log('Selected file:', this.selectedFile);
    
    this.loadEmployeePairs()
  }

  loadEmployeePairs(): void {
    this.employeePairs$ = this.employeeService.getEmployeePairs$(this.selectedFile);
    this.employeePairs$.subscribe({
      next: (pairs) => {
        this.hasRecords = pairs.length > 0;
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
        this.hasRecords = false;
      }
    });
  }
}
