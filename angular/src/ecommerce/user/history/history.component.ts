import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';
import { HistoryOrderDto, HistoryOrderDtoPagedResultDto, HistoryOrderServiceProxy, HistoyOrderRequestModel } from '@shared/service-proxies/service-proxies';



@Component({
    selector: 'history',
    templateUrl: './history.component.html',
    styleUrls: ['./history.component.css'],
    animations: [appModuleAnimation()],
  })

  export class HistoryComponent extends AppComponentBase implements OnInit {
    historyOrders: HistoryOrderDto[];
    request =new HistoyOrderRequestModel();
    constructor(
        injector: Injector,
        private historyService: HistoryOrderServiceProxy
      ) {
        super(injector);
    }

    ngOnInit(): void {
        this.getPaging();
    }

    getPaging(): void {
        this.request.skipCount=0;
        this.request.maxResultCount =20;
        this.historyService.getPaging(this.request).subscribe((result: HistoryOrderDtoPagedResultDto) => {
            this.historyOrders = result.items;
        });
    }
  }
