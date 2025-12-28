import { Component, Injector, OnInit, Renderer2 } from '@angular/core';
import { AppComponentBase } from '../shared/app-component-base';

@Component({
  templateUrl: './app.component.html'
})
export class EcommerceComponent extends AppComponentBase implements OnInit {
  sidebarExpanded: boolean;

  constructor(
    injector: Injector,
    private renderer: Renderer2,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.renderer.addClass(document.body, 'sidebar-mini');

    
  }

}
