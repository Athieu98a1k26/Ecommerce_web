import { Component, Injector, ChangeDetectionStrategy } from '@angular/core';
import { AppComponentBase } from '../../shared/app-component-base';

interface CartItem {
  id: number;
  shopId: number;
  shopName: string;
  shopIcon: string;
  productId: number;
  productName: string;
  productImage: string;
  variation: {
    color?: string;
    size?: string;
  };
  price: number;
  originalPrice?: number;
  quantity: number;
  selected: boolean;
  inStock?: boolean;
}

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CartComponent extends AppComponentBase {
  searchQuery: string = '';
  isLoggedIn: boolean = false;
  voucherCode: string = '';
  discountAmount: number = 0;

  // Sample cart data grouped by shop
  cartItems: CartItem[] = [
    {
      id: 1,
      shopId: 1,
      shopName: 'Shop Thời Trang ABC',
      shopIcon: 'fas fa-store',
      productId: 1,
      productName: 'Áo thun nam cổ tròn chất liệu cotton cao cấp, thoáng mát, nhiều màu sắc',
      productImage: 'https://via.placeholder.com/100x100/ff6b35/ffffff?text=Product+1',
      variation: { color: 'Đỏ', size: 'L' },
      price: 199000,
      originalPrice: 299000,
      quantity: 2,
      selected: true,
      inStock: true
    },
    {
      id: 2,
      shopId: 1,
      shopName: 'Shop Thời Trang ABC',
      shopIcon: 'fas fa-store',
      productId: 2,
      productName: 'Quần jean nam form slim fit, chất liệu denim cao cấp',
      productImage: 'https://via.placeholder.com/100x100/ff8c42/ffffff?text=Product+2',
      variation: { color: 'Xanh', size: 'M' },
      price: 450000,
      originalPrice: 650000,
      quantity: 1,
      selected: true,
      inStock: true
    },
    {
      id: 3,
      shopId: 2,
      shopName: 'Shop Điện Tử XYZ',
      shopIcon: 'fas fa-mobile-alt',
      productId: 3,
      productName: 'Điện thoại smartphone màn hình 6.5 inch, RAM 8GB, bộ nhớ 128GB',
      productImage: 'https://via.placeholder.com/100x100/ffa07a/ffffff?text=Product+3',
      variation: { color: 'Đen' },
      price: 5990000,
      originalPrice: 7990000,
      quantity: 1,
      selected: false,
      inStock: false
    },
    {
      id: 4,
      shopId: 3,
      shopName: 'Shop Mỹ Phẩm Beauty',
      shopIcon: 'fas fa-palette',
      productId: 4,
      productName: 'Kem dưỡng da mặt chống lão hóa, làm trắng da, dưỡng ẩm sâu',
      productImage: 'https://via.placeholder.com/100x100/ffb347/ffffff?text=Product+4',
      variation: {},
      price: 450000,
      originalPrice: 650000,
      quantity: 3,
      selected: true,
      inStock: true
    }
  ];

  constructor(injector: Injector) {
    super(injector);
  }

  // Group items by shop
  get shops() {
    const shopMap = new Map<number, { shopId: number; shopName: string; shopIcon: string; items: CartItem[] }>();
    
    this.cartItems.forEach(item => {
      if (!shopMap.has(item.shopId)) {
        shopMap.set(item.shopId, {
          shopId: item.shopId,
          shopName: item.shopName,
          shopIcon: item.shopIcon,
          items: []
        });
      }
      shopMap.get(item.shopId)!.items.push(item);
    });
    
    return Array.from(shopMap.values());
  }

  // Check if all items in a shop are selected
  isShopAllSelected(shopId: number): boolean {
    const shop = this.shops.find(s => s.shopId === shopId);
    if (!shop || shop.items.length === 0) return false;
    return shop.items.every(item => item.selected);
  }

  // Check if any item in a shop is selected
  isShopPartiallySelected(shopId: number): boolean {
    const shop = this.shops.find(s => s.shopId === shopId);
    if (!shop) return false;
    return shop.items.some(item => item.selected) && !this.isShopAllSelected(shopId);
  }

  // Toggle select all items in a shop
  toggleShopSelectAll(shopId: number): void {
    const shop = this.shops.find(s => s.shopId === shopId);
    if (!shop) return;
    
    const allSelected = this.isShopAllSelected(shopId);
    shop.items.forEach(item => {
      item.selected = !allSelected;
    });
  }

  // Toggle select all items
  isAllSelected(): boolean {
    return this.cartItems.length > 0 && this.cartItems.every(item => item.selected);
  }

  toggleSelectAll(): void {
    const allSelected = this.isAllSelected();
    this.cartItems.forEach(item => {
      item.selected = !allSelected;
    });
  }

  // Toggle select single item
  toggleItemSelect(item: CartItem): void {
    if (item.inStock === false) return;
    // Default inStock to true if undefined
    if (item.inStock === undefined) {
      item.inStock = true;
    }
    item.selected = !item.selected;
  }

  // Update quantity
  updateQuantity(item: CartItem, change: number): void {
    if (item.inStock === false) return;
    const newQuantity = item.quantity + change;
    if (newQuantity >= 1 && newQuantity <= 99) {
      item.quantity = newQuantity;
    }
  }

  // Handle quantity input change
  onQuantityInputChange(event: Event, item: CartItem): void {
    const input = event.target as HTMLInputElement;
    const newQuantity = parseInt(input.value, 10);
    
    if (item.inStock === false) {
      item.quantity = 1;
      input.value = '1';
      return;
    }
    
    if (isNaN(newQuantity) || newQuantity < 1) {
      item.quantity = 1;
      input.value = '1';
    } else if (newQuantity > 99) {
      item.quantity = 99;
      input.value = '99';
    } else {
      item.quantity = newQuantity;
    }
  }

  // Delete item
  deleteItem(itemId: number): void {
    if (confirm('Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng?')) {
      const index = this.cartItems.findIndex(item => item.id === itemId);
      if (index > -1) {
        this.cartItems.splice(index, 1);
      }
    }
  }

  // Get selected items
  get selectedItems(): CartItem[] {
    return this.cartItems.filter(item => item.selected);
  }

  // Get total items count
  get totalItemsCount(): number {
    return this.selectedItems.reduce((sum, item) => sum + item.quantity, 0);
  }

  // Get subtotal (before discount)
  get subtotal(): number {
    return this.selectedItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);
  }

  // Get total (after discount)
  get total(): number {
    return Math.max(0, this.subtotal - this.discountAmount);
  }

  // Apply voucher
  applyVoucher(): void {
    if (this.voucherCode.trim()) {
      // Simulate voucher application
      this.discountAmount = Math.floor(this.subtotal * 0.1); // 10% discount
    }
  }

  // Checkout
  checkout(): void {
    if (this.selectedItems.length === 0) {
      alert('Vui lòng chọn ít nhất một sản phẩm để mua hàng');
      return;
    }
    alert(`Đang xử lý đơn hàng với ${this.totalItemsCount} sản phẩm. Tổng tiền: ${this.formatCurrency(this.total)}`);
  }

  // Format currency
  formatCurrency(amount: number): string {
    return new Intl.NumberFormat('vi-VN').format(amount) + '₫';
  }

  // Check if cart is empty
  get isEmpty(): boolean {
    return this.cartItems.length === 0;
  }
}

