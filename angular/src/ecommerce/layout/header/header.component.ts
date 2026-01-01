import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';
import { AppSessionService } from '@shared/session/app-session.service';
import { Router } from '@node_modules/@angular/router';
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
  selector: 'ecommerce-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class HeaderComponent extends AppComponentBase implements OnInit{
  searchQuery: string = '';
  isLoggedIn: boolean = false;
  cartItemCount: number = 1;
  showNotifications: boolean = false;
  notificationCount: number = 3;
  userName = '';
  userAvatar = 'assets/img/user.png';
  showUserDropdown = false;
  notifications = [
    { title: 'Bạn có đơn hàng mới', time: '2 phút trước' },
    { title: 'Sản phẩm đã được giao', time: '1 giờ trước' },
    { title: 'Khuyến mãi mới hôm nay', time: 'Hôm qua' },
  ];

  languages = [
    { code: 'vi', name: 'Tiếng Việt', flag: 'https://flagcdn.com/w20/vn.png' },
    { code: 'en', name: 'English', flag: 'https://flagcdn.com/w20/us.png' }
  ];
  currentLanguage = this.languages[0];

  constructor(
    injector: Injector,
    private sessionService: AppSessionService,
    private router: Router,
    private authService: AppAuthService,
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.checkLoginStatus();
    const langFromCookie = abp?.utils?.getCookieValue?.('Abp.Localization.CultureName');
    const matchedLang = this.languages.find(l => l.code === langFromCookie);
    if (matchedLang) {
      this.currentLanguage = matchedLang;
    } else {
      this.currentLanguage = this.languages[0];
    }
  }

  setLanguage(langCode: string): void {
    if (langCode !== this.currentLanguage.code) {
      abp.utils.setCookieValue(
        'Abp.Localization.CultureName',
        langCode,
        new Date(new Date().getTime() + 5 * 365 * 86400000), // 5 years
        abp.appPath
      );
      location.reload();
    }
  }

  toggleUserDropdown(): void {
    this.showUserDropdown = !this.showUserDropdown;
  }
  navigateToAccount(): void {
    this.router.navigate(['/ecommerce/account']);
    this.showUserDropdown = false;
  }
  navigateToOrders(): void {
    this.router.navigate(['/ecommerce/orders']);
    this.showUserDropdown = false;
  }

  onLogout(): void {
    this.authService.logout();
    this.showUserDropdown = false;
  }
  checkLoginStatus(){
    const user = this.sessionService.user;
    this.isLoggedIn = !!user;
    if (user) {
      this.userName = user.userName || user.name;
    }
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
