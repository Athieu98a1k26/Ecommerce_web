import {
    Component,
    Injector,
    OnInit,
    Output,
    EventEmitter
  } from '@angular/core';
  import { BsModalRef } from 'ngx-bootstrap/modal';
  import { AppComponentBase } from '@shared/app-component-base';
  import {
    StoreDto,
    StoreServiceProxy
  } from '@shared/service-proxies/service-proxies';
  
  @Component({
    templateUrl: 'create-store-dialog.component.html'
  })
  export class CreateStoreDialogComponent extends AppComponentBase implements OnInit {
    saving = false;
    store: StoreDto = new StoreDto();
  
    @Output() onSave = new EventEmitter<any>();
  
    constructor(
      injector: Injector,
      public _storeService: StoreServiceProxy,
      public bsModalRef: BsModalRef
    ) {
      super(injector);
    }
  
    ngOnInit(): void {
      // You may set store default values here if required
    }
  
    save(): void {
      this.saving = true;
  
      this._storeService.createOrEdit(this.store).subscribe(
        () => {
          this.notify.info(this.l('SavedSuccessfully'));
          this.bsModalRef.hide();
          this.onSave.emit();
        },
        () => {
          this.saving = false;
        }
      );
    }
  }