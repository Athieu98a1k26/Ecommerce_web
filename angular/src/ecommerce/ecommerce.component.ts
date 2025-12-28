import { Component, Injector, OnInit, Renderer2 } from '@angular/core';
import { AppComponentBase } from '../shared/app-component-base';

@Component({
  templateUrl: './ecommerce.component.html'
})
export class EcommerceComponent extends AppComponentBase implements OnInit {
  sidebarExpanded: boolean;

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }

  ngOnInit(): void {

  }

}
