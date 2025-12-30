import { Component, Injector } from '@angular/core';
import { finalize } from 'rxjs/operators';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import {
  PagedListingComponentBase,
} from '@shared/paged-listing-component-base';
import {
  StoreServiceProxy,
  StoreDtoPagedResultDto,
  BaseRequest,
  StoreDto
} from '@shared/service-proxies/service-proxies';
import { CreateStoreDialogComponent } from './create-store/create-store-dialog.component';
import { EditStoreDialogComponent } from './edit-store/edit-store-dialog.component';

@Component({
  templateUrl: './stores.component.html',
  animations: [appModuleAnimation()]
})
export class StoresComponent extends PagedListingComponentBase<StoreDto> {
  stores: StoreDto[] = [];
  keyword = '';

  constructor(
    injector: Injector,
    private _storesService: StoreServiceProxy,
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
    this._storesService
      .getPaging(request)
      .pipe(
        finalize(() => {
          finishedCallback();
        })
      )
      .subscribe((result: StoreDtoPagedResultDto) => {
        this.stores = result.items;
        this.showPaging(result, pageNumber);
      });
  }

  delete(store: StoreDto): void {
    abp.message.confirm(
      this.l('StoreDeleteWarningMessage', store.name),
      undefined,
      (result: boolean) => {
        if (result) {
          this._storesService
            .delete(store.id)
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

  createStore(): void {
    this.showCreateOrEditStoreDialog();
  }

  editStore(store: StoreDto): void {
    this.showCreateOrEditStoreDialog(store.id);
  }

  showCreateOrEditStoreDialog(id?: number): void {
    let createOrEditStoreDialog: BsModalRef;
    if (!id) {
      createOrEditStoreDialog = this._modalService.show(
        CreateStoreDialogComponent,
        {
          class: 'modal-lg',
        }
      );
    } else {
      createOrEditStoreDialog = this._modalService.show(
        EditStoreDialogComponent,
        {
          class: 'modal-lg',
          initialState: {
            id: id,
          },
        }
      );
    }

    createOrEditStoreDialog.content.onSave.subscribe(() => {
      this.refresh();
    });
  }
}
