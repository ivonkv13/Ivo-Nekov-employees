import { Component } from '@angular/core';
import { FileService } from '../../../services/file.service';

@Component({
  selector: 'app-file-upload',
  templateUrl: './file-upload.component.html',
  styleUrls: ['./file-upload.component.css'],
})
export class FileUploadComponent {
  selectedFile: File | null = null;
  uploadProgress = 0;
  buttonToolTip = "THIS WILL REFRESH THE PAGE TO UPDATE THE TABLE. IDEALLY, AN EVENT LISTENER WOULD NOTIFY THE TABLE COMPONENT WHEN A FILE IS UPLOADED, ALLOWING IT TO REFRESH AUTOMATICALLY WITHOUT A FULL RELOAD.";

  constructor(private fileService: FileService) {}

  triggerFileInput(): void { //There is some issue with the Material UI and this is necessary to simulate click event manually.
    document.getElementById('fileInput')?.click();
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  uploadFile(): void {
    if (this.selectedFile) {
      this.fileService.uploadFile(this.selectedFile).subscribe(progress => {
        this.uploadProgress = progress;
      });
    }
  }

  clearProgress(): void {
    this.uploadProgress = 0;
    window.location.reload();
  }
}