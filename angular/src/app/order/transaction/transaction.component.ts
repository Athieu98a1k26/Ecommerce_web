import { Component, Injector, Input } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
} from '@shared/paged-listing-component-base';
import { FileManagerDto, TransactionDto, TransactionDtoPagedResultDto, TransactionRequestModel, TransactionServiceProxy } from '@shared/service-proxies/service-proxies';
import { FileDownloadService } from '@shared/utils/file-download.service';

@Component({
  selector:'transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.scss'],
  animations: [appModuleAnimation()],
})
export class TransactionComponent extends PagedListingComponentBase<TransactionDto> {
  transactions: TransactionDto[] = [];
  orderDetailId: number;
  constructor(
    injector: Injector,
    private transactionService: TransactionServiceProxy,
    private fileDownloadService: FileDownloadService,
  ) {
    super(injector);
  }

  list(
    request: TransactionRequestModel,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.orderDetailId = this.orderDetailId;
    this.transactionService
      .getPaging(request)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: TransactionDtoPagedResultDto) => {
        this.transactions = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  protected delete(entity: TransactionDto): void {
    throw new Error('Method not implemented.');
  }

  downLoadFile(file: FileManagerDto){
    this.fileDownloadService.downloadFile(file);
  }
}
