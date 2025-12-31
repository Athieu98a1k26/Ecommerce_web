import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '../../shared/app-component-base';
import { appModuleAnimation } from '../../shared/animations/routerTransition';
import { PersonServiceProxy, PersonDto, OrderPublicServiceProxy, OrderDto, OrderRequestDto, OrderDtoPagedResultDto } from '../../shared/service-proxies/service-proxies';
import { AppSessionService } from '../../shared/session/app-session.service';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { PagedListingComponentBase, PagedRequestDto } from '@shared/paged-listing-component-base';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'ecommerce-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  animations: [appModuleAnimation()],
})
export class UserComponent extends PagedListingComponentBase<OrderDto> implements OnInit {
  
  userAvatar: string | null = null;
  userName: string | null = null;
  person: PersonDto = new PersonDto();
  tabActive: string = 'Person';
  tabOrder: string =''
  orders: OrderDto[];

  constructor(
    injector: Injector,
    private personService: PersonServiceProxy,
    private sessionService: AppSessionService,
    private authService: AppAuthService,
    private orderService: OrderPublicServiceProxy
  ) {
    super(injector);
  }

  ngOnInit() {
    const user = this.sessionService.user;
    if (user?.id) {
      this.userName = user.userName || user.name;
      this.getPersonInfo(user.id);
    }
  }
  
  getPersonInfo(userId: number): void {
    this.personService.getPerson(userId).subscribe(person => {
      this.person = person;
    });
  }

  protected list(request: PagedRequestDto, pageNumber: number, finishedCallback: Function): void {
    let input = new OrderRequestDto();
    input.email = this.sessionService.user.emailAddress;
    input.maxResultCount = request.maxResultCount;
    input.skipCount = request.skipCount;
    this.orderService
    .getPagingForUser(input)
    .pipe(
      finalize(() => {
        finishedCallback();
      })
    )
    .subscribe((result: OrderDtoPagedResultDto) => {
      this.orders = result.items;
      this.showPaging(result, pageNumber);
    });
  }
  protected delete(entity: OrderDto): void {
    throw new Error('Method not implemented.');
  }

  onSubmitPersonalInfo(){
    this.personService.updatePerson(this.person).subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
      }
    );
  }

  onChangeTab(tab: string){
    this.tabActive = tab;
    if(tab == 'Order'){
      this.refresh()
    }
    
  }

  onLogOut(){
    this.authService.logout();
  }

  viewOrderDetail(order: OrderDto) {
    // Implement logic to show order details, e.g., open modal or navigate
    // alert(JSON.stringify(order));
  }

  // Add more methods for menu actions, tab change, info update, etc. as needed
}
