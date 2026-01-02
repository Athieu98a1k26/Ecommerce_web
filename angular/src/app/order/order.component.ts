import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
} from '@shared/paged-listing-component-base';
import {
  OrderDtoPagedResultDto,
  BaseRequest,
  OrderDto,
  OrderServiceProxy,
  OrderDetailDto,
  OrderDetailRequestModel,
  OrderDetailServiceProxy,
  OrderDetailDtoPagedResultDto
} from '@shared/service-proxies/service-proxies';
import {OrderActionHelper} from '@helper/order-action.helper';
import { TransactionComponent } from './transaction/transaction.component';

@Component({
  templateUrl: './order.component.html',
  animations: [appModuleAnimation()]
})
export class OrderComponent extends PagedListingComponentBase<OrderDto> {
  orders: OrderDto[] = [];
  orderDetails:OrderDetailDto[];
  keyword = '';
  constructor(
    injector: Injector,
    private orderService: OrderServiceProxy,
    private _modalService: BsModalService,
    private orderDetailService: OrderDetailServiceProxy
  ) {
    super(injector);
  }

  list(
    request: BaseRequest,
    pageNumber: number,
    finishedCallback: Function
  ): void {
    request.search = this.keyword;
    this.orderService
      .getPaging(request)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: OrderDtoPagedResultDto) => {
        this.orders = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  getPaginOrderDetail(orderId:number){
    let request = new OrderDetailRequestModel();
    request.orderId = orderId;
    request.skipCount = 0;
    request.maxResultCount=9999;
    this.orderDetailService
      .getPaging(request)
      .subscribe((result: OrderDetailDtoPagedResultDto) => {
        this.orderDetails = result.items;
      });
  }

  toggleRow(order:OrderDto){
    order.isExpanded = !order.isExpanded;
    if(order.isExpanded){
      this.getPaginOrderDetail(order.id);
    }
  }

  showDetail(orderDetail:OrderDetailDto){
    let viewTransactionDialog: BsModalRef;
    viewTransactionDialog = this._modalService.show(
       TransactionComponent,
         {
           class: 'modal-xl',
           initialState: {
            orderDetailId: orderDetail.id,
           },
         }
       );
  }

  canOrder(action: string,orderStatus: string){
    let helper = new OrderActionHelper
    return helper.canOrder(action,orderStatus);
  }

  approveOrder(entity: OrderDto){
    this.orderService.confirmedOrder(entity.id).subscribe(() => {
      abp.notify.success(this.l('ConfirmedSuccess'));
      this.refresh()
    });
  }

  cancelOrder(entity: OrderDto){
    this.orderService.cancelledOrder(entity.id).subscribe(() => {
      abp.notify.success(this.l('CancelledSuccess'));
      this.refresh()
    });
  }

  protected delete(entity: OrderDto): void {
    throw new Error('Method not implemented.');
  }
}
