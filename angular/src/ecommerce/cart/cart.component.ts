import { Component, Injector, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { AppComponentBase } from '../../shared/app-component-base';
import { BaseRequest, CartDto, CartDtoPagedResultDto, CartServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent extends AppComponentBase implements OnInit {
  searchQuery: string = '';
  isLoggedIn: boolean = false;
  voucherCode: string = '';
  discountAmount: number = 0;

  cartItems: CartDto[] = [];
  isLoading: boolean = false;

  constructor(injector: Injector, private cartService: CartServiceProxy) {
    super(injector);
  }
  
  ngOnInit(): void {
    this.isLoading = true;
    let request = new BaseRequest();
    request.skipCount = 0;
    request.maxResultCount = 1000;
    this.cartService.getPaging(request).subscribe({
      next: (result: CartDtoPagedResultDto) => {
        this.cartItems = result.items || [];
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;
      }
    });
  }
  // Check if all items in a shop are selected
  isShopAllSelected(shopId: number): boolean {
    return this.cartItems.every(item => item.selected);
  }

  // Check if any item in a shop is selected
  isShopPartiallySelected(shopId: number): boolean {
    return this.cartItems.some(item => item.selected) && !this.isShopAllSelected(shopId);
  }

  // Toggle select all items in a shop
  toggleShopSelectAll(shopId: number): void {
    const allSelected = this.isShopAllSelected(shopId);
    this.cartItems.forEach(item => {
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
  toggleItemSelect(item: CartDto): void {
    if (item.inStock === false) return;
    // Default inStock to true if undefined
    if (item.inStock === undefined) {
      item.inStock = true;
    }
    item.selected = !item.selected;
  }

  // Update quantity
  updateQuantity(item: CartDto, change: number): void {
    if (item.inStock === false) return;
    const newQuantity = item.quantity + change;
    if (newQuantity >= 1 && newQuantity <= 99) {
      item.quantity = newQuantity;
    }
  }

  // Handle quantity input change
  onQuantityInputChange(event: Event, item: CartDto): void {
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
    this.cartService.delete(itemId).subscribe({
      next: () => {
        this.cartItems = this.cartItems.filter(item => item.id !== itemId);
      },
      error: () => {
        this.notify.error(this.l('FailedToDeleteItem'));
      }
    });
  }

  // Get selected items
  get selectedItems(): CartDto[] {
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

