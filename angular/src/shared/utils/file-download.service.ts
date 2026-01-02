
import { AppConsts } from '../AppConsts'
import { Injectable } from '@angular/core';
import { FileManagerDto } from '../service-proxies/service-proxies';

@Injectable()
export class FileDownloadService {
    downloadFile(file: FileManagerDto) {
        const url =
            AppConsts.remoteServiceBaseUrl +
            '/File/DownloadFile?extension=' +
            encodeURIComponent(file.extension??'') +
            '&id=' +
            encodeURIComponent(file.id) +
            '&name=' +
            encodeURIComponent(file.name??'');
        location.href = url; //TODO: This causes reloading of same page in Firefox
    }
}
