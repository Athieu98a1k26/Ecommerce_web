import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { appModuleAnimation } from '../../shared/animations/routerTransition';
import { AppComponentBase } from '../../shared/app-component-base';
import { OwlOptions } from 'ngx-owl-carousel-o';
import {
  ProductStorePublicServiceProxy,
  ProductStoreDto,
  ProductStoreDtoPagedResultDto,
  BaseRequest
} from '@shared/service-proxies/service-proxies';
import { environment } from '../../environments/environment';

interface Category {
  name: string;
  icon: string;
}

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends AppComponentBase implements OnInit {
  
  cartItemCount: number = 3;
  isLoggedIn: boolean = false;

  customOptions: OwlOptions = {
    loop: true,
    items: 3,
    autoplay: true,
    mouseDrag: true,
    touchDrag: false,
    pullDrag: true,
    dots: true,
    navSpeed: 700,
    navText: ['', ''],
    responsive: {
      0: {
        items: 1
      },
      400: {
        items: 1
      },
      740: {
        items: 1
      },
      940: {
        items: 1
      }
    },
    nav: false
  }

  banners = [
    { title: 'Sale 12.12', img: 'assets/img/banner1.jpg' },
    { title: 'Giảm 50%', img: 'assets/img/banner2.jpg' },
    { title: 'Flash Sale', img: 'assets/img/banner3.jpg' }
  ];

  brands = [
    { name: 'Apple', slug: 'apple', logo: 'assets/img/brands/apple.png' },
    { name: 'Samsung', slug: 'samsung', logo: 'assets/img/brands/samsung.png' },
    { name: 'Xiaomi', slug: 'xiaomi', logo: 'assets/img/brands/xiaomi.png' },
    { name: 'Honor', slug: 'honor', logo: 'assets/img/brands/honor.png' },
    { name: 'Oppo', slug: 'oppo', logo: 'assets/img/brands/oppo.png' },
    { name: 'Tecno', slug: 'tecno', logo: 'assets/img/brands/tecno.png' },
    { name: 'Vivo', slug: 'vivo', logo: 'assets/img/brands/vivo.png' },
    { name: 'Nokia', slug: 'nokia', logo: 'assets/img/brands/nokia.png' },
    { name: 'Realme', slug: 'realme', logo: 'assets/img/brands/realme.png' },
    { name: 'Infinix', slug: 'infinix', logo: 'assets/img/brands/infinix.png' }
  ];

  products: ProductStoreDto[] = [];
  isProductsLoading: boolean = false;

  constructor(
    injector: Injector,
    private productStorePublicService: ProductStorePublicServiceProxy
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.getFeaturedProducts();
  }

  getFeaturedProducts(): void {
    this.isProductsLoading = true;
    const input = new BaseRequest();
    // Set paging as you need (for example, maxResultCount, skipCount)
    input.storeCode =environment.storeCode;
    input.maxResultCount = 12;
    input.skipCount = 0;

    this.productStorePublicService.getPagingFeaturedProduct(input)
      .subscribe({
        next: (result: ProductStoreDtoPagedResultDto) => {
          this.products = result.items || [];
          this.isProductsLoading = false;
        },
        error: () => {
          this.products = [];
          this.isProductsLoading = false;
        }
      });
  }
  trackByFn(index: number, item: any) {
    return item.id; // hoặc index nếu không có id
  }
  goToBrand(brand: any) {
    //this.router.navigate(['/category', brand.slug]);
  }
}
