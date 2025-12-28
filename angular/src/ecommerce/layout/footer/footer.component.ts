import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';



@Component({
    selector: 'ecommerce-footer',
    templateUrl: './footer.component.html',
    styleUrls: ['./footer.component.css'],
    animations: [appModuleAnimation()],
    changeDetection: ChangeDetectionStrategy.OnPush
  })

  export class FooterComponent extends AppComponentBase {
    constructor(
        injector: Injector
      ) {
        super(injector);
      }
  }
