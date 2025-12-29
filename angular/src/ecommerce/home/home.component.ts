import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { appModuleAnimation } from '../../shared/animations/routerTransition';
import { AppComponentBase } from '../../shared/app-component-base';
import { OwlOptions } from 'ngx-owl-carousel-o';

interface Category {
  name: string;
  icon: string;
}

interface Product {
  id: number;
  name: string;
  image: string;
  price: number;
  originalPrice?: number;
  discount: number;
  rating: number;
  sold: string;
  freeShipping: boolean;
}

@Component({
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends AppComponentBase {
  
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

  goToBrand(brand: any) {
    //this.router.navigate(['/category', brand.slug]);
  }
  
  products: Product[] = [
    {
      id: 1,
      name: 'Áo thun nam cổ tròn chất liệu cotton cao cấp, thoáng mát, nhiều màu sắc',
      image: 'assets/img/product/iphone_17pro.png',
      price: 199000,
      originalPrice: 299000,
      discount: 30,
      rating: 4,
      sold: '2.3k',
      freeShipping: true
    },
    {
      id: 2,
      name: 'Điện thoại smartphone màn hình 6.5 inch, RAM 8GB, bộ nhớ 128GB',
      image: 'assets/img/product/iphone_17pro.png',
      price: 5990000,
      originalPrice: 7990000,
      discount: 25,
      rating: 5,
      sold: '5.1k',
      freeShipping: true
    },
    {
      id: 3,
      name: 'Máy xay sinh tố đa năng, công suất cao, an toàn cho sức khỏe',
      image: 'assets/img/product/iphone_17pro.png',
      price: 890000,
      originalPrice: 1200000,
      discount: 26,
      rating: 4,
      sold: '1.8k',
      freeShipping: false
    },
    {
      id: 4,
      name: 'Kem dưỡng da mặt chống lão hóa, làm trắng da, dưỡng ẩm sâu',
      image: 'assets/img/product/iphone_17pro.png',
      price: 450000,
      originalPrice: 650000,
      discount: 31,
      rating: 4,
      sold: '3.2k',
      freeShipping: true
    },
    {
      id: 5,
      name: 'Laptop gaming hiệu năng cao, card đồ họa rời, màn hình 15.6 inch',
      image: 'assets/img/product/iphone_17pro.png',
      price: 18990000,
      originalPrice: 24990000,
      discount: 24,
      rating: 5,
      sold: '892',
      freeShipping: true
    },
    {
      id: 6,
      name: 'Quần jean nam form slim fit, chất liệu denim cao cấp, bền đẹp',
      image: 'assets/img/product/iphone_17pro.png',
      price: 450000,
      originalPrice: 650000,
      discount: 31,
      rating: 4,
      sold: '1.5k',
      freeShipping: true
    },
    {
      id: 7,
      name: 'Tai nghe không dây chống ồn chủ động, pin lâu, chất lượng âm thanh cao',
      image: 'assets/img/product/iphone_17pro.png',
      price: 1290000,
      originalPrice: 1990000,
      discount: 35,
      rating: 4,
      sold: '4.7k',
      freeShipping: true
    },
    {
      id: 8,
      name: 'Bàn chải đánh răng điện tự động, nhiều chế độ, sạc pin USB',
      image: 'assets/img/product/iphone_17pro.png',
      price: 350000,
      originalPrice: 500000,
      discount: 30,
      rating: 4,
      sold: '2.1k',
      freeShipping: true
    },
    {
      id: 9,
      name: 'Son môi lì không trôi màu, nhiều màu sắc, bền màu cả ngày',
      image: 'assets/img/product/iphone_17pro.png',
      price: 120000,
      originalPrice: 180000,
      discount: 33,
      rating: 5,
      sold: '6.8k',
      freeShipping: true
    },
    {
      id: 10,
      name: 'Máy tính bảng màn hình 10 inch, bút cảm ứng, pin lâu',
      image: 'assets/img/product/iphone_17pro.png',
      price: 4990000,
      originalPrice: 6990000,
      discount: 29,
      rating: 4,
      sold: '1.2k',
      freeShipping: true
    }
  ];

  constructor(injector: Injector) {
    super(injector);
  }
}
