import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '../../shared/app-component-base';
import { appModuleAnimation } from '../../shared/animations/routerTransition';

@Component({
    selector: 'ecommerce-user',
    templateUrl: './user.component.html',
    styleUrls: ['./user.component.css'],
    animations: [appModuleAnimation()],
    changeDetection: ChangeDetectionStrategy.OnPush
  })

  export class UserComponent extends AppComponentBase {
    constructor(
        injector: Injector
      ) {
        super(injector);
      }
  }
