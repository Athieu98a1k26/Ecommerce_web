import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
  PagedRequestDto
} from '@shared/paged-listing-component-base';
import {
  ProductServiceServiceProxy,
  ProductRequestModel,
  ProductModelPagedResultDto,
  BaseRequest,
  ProductModel
} from '@shared/service-proxies/service-proxies';
import { CreateProductDialogComponent } from './create-product/create-product-dialog.component';
import { EditProductDialogComponent } from './edit-product/edit-product-dialog.component';

@Component({
  templateUrl: './products.component.html',
  animations: [appModuleAnimation()]
})
export class ProductsComponent extends PagedListingComponentBase<ProductRequestModel> {
  products: ProductRequestModel[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _productsService: ProductServiceServiceProxy,
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

    this._productsService
      .getPaging(request)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: ProductModelPagedResultDto) => {
        this.products = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  delete(product: ProductModel): void {
    abp.message.confirm(
      this.l('ProductDeleteWarningMessage', product.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._productsService
            .delete(product.id)
            .pipe(
              finalize(() => {
                abp.notify.success(this.l('SuccessfullyDeleted'));
                this.refresh();
              })
            )
            .subscribe(() => {});
        }
      }
    );
  }

  createProduct(): void {
    this.showCreateOrEditProductDialog();
  }

  editProduct(product: ProductModel): void {
    this.showCreateOrEditProductDialog(product.id);
  }

  showCreateOrEditProductDialog(id?: number): void {
    let createOrEditProductDialog: BsModalRef;
    if (!id) {
      createOrEditProductDialog = this._modalService.show(
        CreateProductDialogComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditProductDialog = this._modalService.show(
        EditProductDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditProductDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }
}
