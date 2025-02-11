import { HttpClient, HttpEvent, HttpEventType } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class FileService {

  private baseUrl = 'https://localhost:7288/api/File';
  private apiUrl = 'https://localhost:7288/api/File/UploadFile';
  private api2Url = 'https://localhost:7288/api/File/GetAllFilesInFolder';

  constructor(private http: HttpClient) { }

  uploadFile(file: File): Observable<number> {
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<HttpEvent<any>>(`${this.baseUrl}/UploadFile`, formData, {
      reportProgress: true,
      observe: 'events',
    }).pipe(
      map(event => this.getProgress(event))
    );
  }

  private getProgress(event: HttpEvent<any>): number {
    switch (event.type) {
      case HttpEventType.UploadProgress:
        return Math.round((100 * event.loaded) / (event.total || 1));
      case HttpEventType.Response:
        return 100;
      default:
        return 0;
    }
  }

  getAllFiles(): Observable<string[]> {
    return this.http.get<{ files: string[] }>(`${this.baseUrl}/GetAllFilesInFolder`).pipe(
      map(response => response.files)
    );
}
}
