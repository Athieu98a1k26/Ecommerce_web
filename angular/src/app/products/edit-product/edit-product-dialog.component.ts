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
  ProductServiceServiceProxy,
  ProductModel
} from '@shared/service-proxies/service-proxies';

@Component({
  templateUrl: 'edit-product-dialog.component.html'
})
export class EditProductDialogComponent extends AppComponentBase
  implements OnInit {
  saving = false;
  product: ProductModel = new ProductModel();
  id: number;

  @Output() onSave = new EventEmitter<any>();

  constructor(
    injector: Injector,
    public _productService: ProductServiceServiceProxy,
    public bsModalRef: BsModalRef
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this._productService.get(this.id).subscribe((result: ProductModel) => {
      this.product = result;
    });
  }

  save(): void {
    this.saving = true;

    this._productService.createOrEdit(this.product).subscribe(
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
