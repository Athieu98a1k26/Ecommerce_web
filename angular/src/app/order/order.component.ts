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
  OrderServiceProxy
} from '@shared/service-proxies/service-proxies';
import {OrderActionHelper} from './order-action.helper';
import { ViewOrderComponent } from './view-order/view-order.component';

@Component({
  templateUrl: './order.component.html',
  animations: [appModuleAnimation()]
})
export class OrderComponent extends PagedListingComponentBase<OrderDto> {
  orders: OrderDto[] = [];
  keyword = '';
  constructor(
    injector: Injector,
    private orderService: OrderServiceProxy,
    private _modalService: BsModalService
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

  showDetail(entity: OrderDto){
    let viewOrderDetailDialog: BsModalRef;
    viewOrderDetailDialog = this._modalService.show(
      ViewOrderComponent,
        {
          class: 'modal-xl',
          initialState: {
            orderId: entity.id,
          },
        }
      );
  }

  protected delete(entity: OrderDto): void {
    throw new Error('Method not implemented.');
  }
}
