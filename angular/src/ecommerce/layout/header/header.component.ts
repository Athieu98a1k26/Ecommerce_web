import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';


@Component({
    selector: 'ecommerce-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.scss'],
    animations: [appModuleAnimation()],
    changeDetection: ChangeDetectionStrategy.OnPush
  })

  export class HeaderComponent extends AppComponentBase {
    searchQuery: string = '';
    isLoggedIn: boolean = false;
    cartItemCount: number = 1;
    notificationCount: number = 1;
    constructor(
      injector: Injector
    ) {
      super(injector);
    }
  }


