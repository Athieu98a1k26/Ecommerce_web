import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '../../shared/app-component-base';
import { appModuleAnimation } from '../../shared/animations/routerTransition';

@Component({
    selector: 'product-search',
    templateUrl: './product-search.component.html',
    styleUrls: ['./product-search.component.scss'],
    animations: [appModuleAnimation()],
    changeDetection: ChangeDetectionStrategy.OnPush
  })

  export class ProductSearchComponent extends AppComponentBase {
    constructor(
        injector: Injector
      ) {
        super(injector);
      }
  }
