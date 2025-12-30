
import { Component, Injector, ChangeDetectionStrategy, Input } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';
import { ProductStoreDto } from '@shared/service-proxies/service-proxies';

@Component({
    selector: 'product-item',
    templateUrl: './product-item.component.html',
    styleUrls: ['./product-item.component.scss'],
    animations: [appModuleAnimation()],
    changeDetection: ChangeDetectionStrategy.OnPush
  })

  export class ProductItemComponent extends AppComponentBase {
    @Input() product: ProductStoreDto;
    
    constructor(
        injector: Injector
      ) {
        super(injector);
      }
  }
