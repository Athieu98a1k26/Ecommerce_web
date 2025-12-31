import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '../../shared/app-component-base';
import { appModuleAnimation } from '../../shared/animations/routerTransition';
import { PersonServiceProxy, PersonDto } from '../../shared/service-proxies/service-proxies';
import { AppSessionService } from '../../shared/session/app-session.service';

interface Order {
  productImage: string;
  productName: string;
  productVariant: string;
  price: number;
  statusClass: string;
  statusText: string;
  // add more fields as needed
}

@Component({
  selector: 'ecommerce-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserComponent extends AppComponentBase implements OnInit {
  userAvatar: string | null = null;
  userName: string | null = null;
  person:PersonDto =null;
  userDob: string = '';

  orders: Order[] = [
    // Example data, replace with real data from API/service
    {
      productImage: 'assets/img/example-product1.jpg',
      productName: 'Sản phẩm 1',
      productVariant: 'Phiên bản A',
      price: 200000,
      statusClass: 'pending',
      statusText: 'Chờ xác nhận'
    },
    {
      productImage: 'assets/img/example-product2.jpg',
      productName: 'Sản phẩm 2',
      productVariant: 'Phiên bản B',
      price: 350000,
      statusClass: 'delivering',
      statusText: 'Đang giao'
    },
    {
      productImage: 'assets/img/example-product3.jpg',
      productName: 'Sản phẩm 3',
      productVariant: 'Phiên bản C',
      price: 400000,
      statusClass: 'completed',
      statusText: 'Hoàn thành'
    }
    // Add more orders as needed
  ];

  constructor(
    injector: Injector,
    private personService: PersonServiceProxy,
    private sessionService: AppSessionService
  ) {
    super(injector);
  }

  ngOnInit() {
    const user = this.sessionService.user;
    if (user && user.id) {
      this.getPersonInfo(user.id);
      this.userName = user.userName || user.name || 'Người dùng';
    } else {
      // No logged in user; fallback or redirect as needed.
    }
  }

  getPersonInfo(userId: number): void {
    this.personService.getPerson(userId).subscribe({
      next: (person: PersonDto) => {
        this.userAvatar = null;
        this.person = person;
       
      },
    });
  }

  onSubmitPersonalInfo(){
    this.personService.updatePerson(this.person).subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
      }
    );
  }

  viewOrderDetail(order: Order) {
    // Implement logic to show order details, e.g., open modal or navigate
    // alert(JSON.stringify(order));
  }

  // Add more methods for menu actions, tab change, info update, etc. as needed
}
