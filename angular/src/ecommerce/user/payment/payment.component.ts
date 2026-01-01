import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';
@Component({
    selector: 'payment',
    templateUrl: './payment.component.html',
    styleUrls: ['./payment.component.scss'],
    animations: [appModuleAnimation()],
  })

  export class PaymentComponent extends AppComponentBase {
    constructor(
        injector: Injector
      ) {
        super(injector);
      }
  }
