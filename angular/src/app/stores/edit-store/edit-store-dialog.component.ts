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
  StoreServiceProxy,
  StoreDto
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-store-dialog.component.html'
})
export class EditStoreDialogComponent extends AppComponentBase implements OnInit {
  saving = false;
  store: StoreDto = new StoreDto();
  id: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _storeService: StoreServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._storeService.get(this.id).subscribe((result: StoreDto) => {
      this.store = result;
    });
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
