import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
} from '@shared/paged-listing-component-base';
import {
  BaseRequest,
  OrderDetailDto,
  OrderDetailDtoPagedResultDto,
  OrderDetailRequestModel,
  OrderDetailServiceProxy,
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './view-order.component.html',
  animations: [appModuleAnimation()]
})
export class ViewOrderComponent extends PagedListingComponentBase<OrderDetailDto> {
  orderDetails: OrderDetailDto[] = [];
  orderId: number;
  orderDetailId: number;
  constructor(
    injector: Injector,
    private orderDetailService: OrderDetailServiceProxy,
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

  protected delete(entity: OrderDetailDto): void {
    throw new Error('Method not implemented.');
  }

  showDetail(orderDetail:OrderDetailDto){
    this.orderDetailId = orderDetail.id;
  }
}
