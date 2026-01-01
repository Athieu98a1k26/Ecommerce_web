import { Component, Injector, ChangeDetectionStrategy, Input } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';
@Component({
    selector: 'file-upload',
    templateUrl: './file-upload.component.html',
    styleUrls: ['./file-upload.component.scss'],
    animations: [appModuleAnimation()],
  })

  export class UploadFileComponent extends AppComponentBase {
    constructor(
        injector: Injector
      ) {
        super(injector);
      }

  @Input() maxFiles = 1;
  maxFileSize = 5 * 1024 * 1024; // 5MB

  files: File[] = [];
  // chọn file
  onFileChange(event: any): void {
    const fileList: FileList = event.target.files;

    for (let i = 0; i < fileList.length; i++) {
      if (this.files.length >= this.maxFiles) {
        this.notify.warn(this.l('MaxFileReached'));
        break;
      }

      const file = fileList[i];

      if (file.size > this.maxFileSize) {
        this.notify.warn(
          this.l('FileTooLarge', file.name)
        );
        continue;
      }

      this.files.push(file);
    }

    // reset input để chọn lại cùng file
    event.target.value = '';
  }

  // xóa file
  removeFile(index: number): void {
    this.files.splice(index, 1);
  }

  // upload lên server
  uploadFiles(): void {
    if (!this.files.length) {
      this.notify.warn(this.l('NoFileSelected'));
      return;
    }

    const formData = new FormData();
    this.files.forEach(file => {
      formData.append('files', file);
    });

    
  }
}
