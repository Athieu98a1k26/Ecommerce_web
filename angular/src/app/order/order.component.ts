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
  showCreateOrEditProductDialog(id?: number): void {
    
  }

  approveOrder(entity: OrderDto){

  }

  cancelOrder(entity: OrderDto){

  }

  showDetail(entity: OrderDto){

  }

  protected delete(entity: OrderDto): void {
    throw new Error('Method not implemented.');
  }
}
