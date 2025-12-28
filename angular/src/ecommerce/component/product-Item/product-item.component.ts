
import { Component, Injector, ChangeDetectionStrategy, Input } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';

interface Product {
    id: number;
    name: string;
    image: string;
    price: number;
    originalPrice?: number;
    discount: number;
    rating: number;
    sold: string;
    freeShipping: boolean;
}

@Component({
    selector: 'product-item',
    templateUrl: './product-item.component.html',
    styleUrls: ['./product-item.component.scss'],
    animations: [appModuleAnimation()],
    changeDetection: ChangeDetectionStrategy.OnPush
  })

  export class ProductItemComponent extends AppComponentBase {
    @Input() product: Product;
    
    constructor(
        injector: Injector
      ) {
        super(injector);
      }
  }
