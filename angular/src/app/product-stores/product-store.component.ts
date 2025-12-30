import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
} from '@shared/paged-listing-component-base';
import {
  ProductStoreServiceProxy,
  ProductStoreDtoPagedResultDto,
  BaseRequest,
  ProductStoreDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: './product-store.component.html',
  animations: [appModuleAnimation()]
})
export class ProductStoresComponent extends PagedListingComponentBase<ProductStoreDto> {
  productStores: ProductStoreDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _productStoresService: ProductStoreServiceProxy,
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
    this._productStoresService
      .getPaging(request)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: ProductStoreDtoPagedResultDto) => {
        this.productStores = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  delete(productStore: ProductStoreDto): void {
    // abp.message.confirm(
    //   this.l('ProductStoreDeleteWarningMessage', productStore.name),
    //   undefined,
    //   (result: boolean) => {
    //     if (result) {
    //       this._productStoresService
    //         .delete(productStore.id)
    //         .pipe(
    //           finalize(() => {
    //             abp.notify.success(this.l('SuccessfullyDeleted'));
    //             this.refresh();
    //           })
    //         )
    //         .subscribe(() => {});
    //     }
    //   }
    // );
  }

  createProductStore(): void {
    this.showCreateOrEditProductStoreDialog();
  }

  editProductStore(productStore: ProductStoreDto): void {
    this.showCreateOrEditProductStoreDialog(productStore.id);
  }

  showCreateOrEditProductStoreDialog(id?: number): void {
    // let createOrEditProductStoreDialog: BsModalRef;
    // if (!id) {
    //   createOrEditProductStoreDialog = this._modalService.show(
    //     CreateProductStoreDialogComponent,
    //     {
    //       class: 'modal-lg',
    //     }
    //   );
    // } else {
    //   createOrEditProductStoreDialog = this._modalService.show(
    //     EditProductStoreDialogComponent,
    //     {
    //       class: 'modal-lg',
    //       initialState: {
    //         id: id,
    //       },
    //     }
    //   );
    // }

    // createOrEditProductStoreDialog.content.onSave.subscribe(() => {
    //   this.refresh();
    // });
  }
}
