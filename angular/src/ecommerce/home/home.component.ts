import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { appModuleAnimation } from '../../shared/animations/routerTransition';
import { AppComponentBase } from '../../shared/app-component-base';

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
  styleUrls: ['./home.component.css','../css/base.css'],
  animations: [appModuleAnimation()],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class HomeComponent extends AppComponentBase {
  
  cartItemCount: number = 3;
  isLoggedIn: boolean = false;

  categories: Category[] = [
    { name: 'Thời trang', icon: 'fas fa-tshirt' },
    { name: 'Điện thoại', icon: 'fas fa-mobile-alt' },
    { name: 'Gia dụng', icon: 'fas fa-home' },
    { name: 'Mỹ phẩm', icon: 'fas fa-palette' },
    { name: 'Laptop', icon: 'fas fa-laptop' }
  ];

  products: Product[] = [
    {
      id: 1,
      name: 'Áo thun nam cổ tròn chất liệu cotton cao cấp, thoáng mát, nhiều màu sắc',
      image: 'https://via.placeholder.com/300x300/ff6b35/ffffff?text=Product+1',
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
      image: 'https://via.placeholder.com/300x300/ff8c42/ffffff?text=Product+2',
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
      image: 'https://via.placeholder.com/300x300/ffa07a/ffffff?text=Product+3',
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
      image: 'https://via.placeholder.com/300x300/ffb347/ffffff?text=Product+4',
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
      image: 'https://via.placeholder.com/300x300/ffcc99/ffffff?text=Product+5',
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
      image: 'https://via.placeholder.com/300x300/ff6b35/ffffff?text=Product+6',
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
      image: 'https://via.placeholder.com/300x300/ff8c42/ffffff?text=Product+7',
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
      image: 'https://via.placeholder.com/300x300/ffa07a/ffffff?text=Product+8',
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
      image: 'https://via.placeholder.com/300x300/ffb347/ffffff?text=Product+9',
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
      image: 'https://via.placeholder.com/300x300/ffcc99/ffffff?text=Product+10',
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
