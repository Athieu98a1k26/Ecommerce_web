import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';

@Component({
  selector: 'ecommerce-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent extends AppComponentBase {
  searchQuery: string = '';
  isLoggedIn: boolean = false;
  cartItemCount: number = 1;
  showNotifications: boolean = false;
  notificationCount: number = 3;

  notifications = [
    { title: 'Bạn có đơn hàng mới', time: '2 phút trước' },
    { title: 'Sản phẩm đã được giao', time: '1 giờ trước' },
    { title: 'Khuyến mãi mới hôm nay', time: 'Hôm qua' },
  ];

  constructor(injector: Injector) {
    super(injector);
  }

  toggleNotifications(event: Event): void {
    event.stopPropagation();
    this.showNotifications = !this.showNotifications;
  }

  closeDropdown(): void {
    this.showNotifications = false;
  }

  onSearch(): void {
    // Điều hướng sang trang tìm kiếm sản phẩm, truyền searchQuery nếu có
    window.location.href = `/ecommerce/product-search`;
  }
}
