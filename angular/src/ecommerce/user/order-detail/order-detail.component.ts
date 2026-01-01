import { Component, Injector, Input } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
} from '@shared/paged-listing-component-base';
import { PaymentComponent } from '../payment/payment.component';
import {TransactionActionHelper} from '@helper/transaction-action.hepler';
import {
  BaseRequest,
  OrderDetailDto,
  OrderDetailDtoPagedResultDto,
  OrderDetailRequestModel,
  OrderDetailServiceProxy,
  TransactionDto,
  TransactionDtoPagedResultDto,
  TransactionRequestModel,
  TransactionServiceProxy,
} from '@shared/service-proxies/service-proxies';

@Component({
  selector:'order-detail',
  templateUrl: './order-detail.component.html',
  animations: [appModuleAnimation()]
})
export class OrderDetailComponent extends PagedListingComponentBase<OrderDetailDto> {
  orderDetails: OrderDetailDto[] = [];
  @Input() orderId: number;
  listTransaction: TransactionDto[] = [];
  constructor(
    injector: Injector,
    private orderDetailService: OrderDetailServiceProxy,
    private transactionService: TransactionServiceProxy,
    private modalService: BsModalService
  ) {
    super(injector);
  }

  list(
    request: OrderDetailRequestModel,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.orderId = this.orderId;
    this.orderDetailService
      .getPaging(request)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: OrderDetailDtoPagedResultDto) => {
        this.orderDetails = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  getPagingTransaction(orderDetailId: number): void {
    let request = new TransactionRequestModel();
    request.maxResultCount = 999;
    request.skipCount = 0;
    request.orderDetailId = orderDetailId;
    this.transactionService.getPaging(request).subscribe((result: TransactionDtoPagedResultDto) => {
      this.listTransaction = result.items;
    });
  }

  protected delete(entity: OrderDetailDto): void {
    throw new Error('Method not implemented.');
  }

  payTransaction(transaction:TransactionDto){
    let paymentDialog: BsModalRef;
    paymentDialog = this.modalService.show(
      PaymentComponent,
        {
          class: 'modal-xl',
          initialState: {
            transactionId:transaction.id
          },
        }
      );

      paymentDialog.content.onSave.subscribe(() => {
        this.refresh();
      });
  }

  canTransaction(action: string,transactionStatus: string){
    let helper = new TransactionActionHelper();
    return helper.canTransaction(action,transactionStatus);
  }

  toggleRow(orderDetail:OrderDetailDto){
    orderDetail.isExpanded = !orderDetail.isExpanded;
    if(orderDetail.isExpanded){
      this.getPagingTransaction(orderDetail.id);
    }
  }
}
