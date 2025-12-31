import { Component, OnInit, HostListener, Injector } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { 
  ProductStoreDto,
  ProductStoreDetailDto,
  ProvinceServiceProxy,
  ProvinceDto,
  CreateUpdateOrderDto,
  OrderDetailDto,
  OrderPublicServiceProxy, } from '@shared/service-proxies/service-proxies';
import { BsModalRef } from '@node_modules/ngx-bootstrap/modal';
import {
  NotifyService,
} from 'abp-ng2-module';
import { AppComponentBase } from '@shared/app-component-base';
@Component({
  selector: 'app-product',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.scss']
})
export class OrderComponent extends AppComponentBase implements OnInit {

  productStoreId: number;
  quantity = 1;
  productId: number = null;
  saving = false;
  notify: NotifyService;
  currentImageIndex = 0;
  selectedDetail:ProductStoreDetailDto;
  countDetail: Number = 0;
  deliveryMethodHome: string = 'HomeDelivery';
  deliveryMethodStore: string = 'InStore';
  customerInfo = {
    fullName: '',
    phone: '',
    email: ''
  };
  deliveryMethod: string = this.deliveryMethodHome;
  deliveryAddress = {
    province: '',
    ward:''
  };
  notes = '';
  showImageModal = false;
  modalImageIndex = 0;
  searchQuery = '';

  product: ProductStoreDto = null;
  listProvince: ProvinceDto[]
  listWard: ProvinceDto[]
  constructor(
    injector: Injector,
    private router: Router,
    private provinceApi: ProvinceServiceProxy,
    private orderApi: OrderPublicServiceProxy,
    public bsModalRef: BsModalRef
  ) {
      super(injector);
  }

  ngOnInit(): void {
    this.getListProvince();
  }

  getListProvince(){
    this.provinceApi.getAllProvince().subscribe({
      next: (result: ProvinceDto[]) => {
        this.listProvince = result;
      },
      
    });
  }

  getAllWardByProvince(){
    this.provinceApi.getAllWard(this.deliveryAddress.province).subscribe({
      next: (result: ProvinceDto[]) => {
        this.listWard = result;
      },
      
    });
  }

  onProvinceChange(){
    this.listWard = [];
    this.getAllWardByProvince();
  }

  goToCart(): void {
    this.router.navigate(['/ecommerce/cart']);
  }

  get activeCapacityCodeDetails() {
    return this.product.listProductStoreDetailDto.filter(d => d.capacityCode);
  }

  get activeColorCodeDetails() {
    return this.product.listProductStoreDetailDto.filter(d => d.colorCode);
  }

  save(): void {
    this.saving = true;
    let input = new CreateUpdateOrderDto();
    input.note = this.notes?.trim();
    input.deliveryMethod = this.deliveryMethod;
    input.phoneNumber = this.customerInfo.phone?.trim();
    input.fullName = this.customerInfo.fullName?.trim();
    input.email = this.customerInfo.email?.trim();
    input.provinceCode = this.deliveryAddress.province;

    input.listOrderDetailDto = [];

    let orderDetailDto = new OrderDetailDto();
    orderDetailDto.productStoreDetailId = this.selectedDetail.id;
    orderDetailDto.count = this.quantity;

    input.listOrderDetailDto.push(orderDetailDto)

    this.orderApi.createOrEdit(input).subscribe(
      () => {
        this.notify.info(this.l('SavedSuccessfully'));
        this.bsModalRef.hide();
      },
      () => {
        this.saving = false;
      }
    );
    
  }

  nextImage(): void {
    if (!this.product?.listProductStoreDetailDto?.length) return;
    this.currentImageIndex = (this.currentImageIndex + 1) % this.product.listProductStoreDetailDto.length;
  }

  prevImage(): void {
    if (!this.product?.listProductStoreDetailDto?.length) return;
    this.currentImageIndex = this.currentImageIndex === 0 
      ? this.product.listProductStoreDetailDto.length - 1 
      : this.currentImageIndex - 1;
  }

  goToImage(index: number): void {
    this.currentImageIndex = index;
  }

  selectVersion(selectedDetail: ProductStoreDetailDto): void {
    this.selectedDetail = selectedDetail;
  }

  selectColor(selectedDetail: ProductStoreDetailDto): void {
    this.selectedDetail = selectedDetail;
  }

  increaseQuantity(): void {
    this.quantity++;
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  openImageModal(index: number): void {
    this.modalImageIndex = index;
    this.currentImageIndex = index;
    this.showImageModal = true;
    document.body.style.overflow = 'hidden';
  }

  closeImageModal(): void {
    this.showImageModal = false;
    document.body.style.overflow = '';
  }

  nextModalImage(): void {
    if (!this.product?.listProductStoreDetailDto?.length) return;
    const currentDetail = this.selectedDetail;
    const currentIndex = this.product.listProductStoreDetailDto.findIndex(
      d => d === currentDetail
    );
    const length = this.product.listProductStoreDetailDto.length;
    const nextIndex = (currentIndex + 1) % length;
    this.selectedDetail = this.product.listProductStoreDetailDto[nextIndex];
    this.modalImageIndex = nextIndex;
    this.currentImageIndex = nextIndex;
  }

  prevModalImage(): void {
    if (!this.product?.listProductStoreDetailDto?.length) return;
    const currentDetail = this.selectedDetail;
    const currentIndex = this.product.listProductStoreDetailDto.findIndex(
      d => d === currentDetail
    );
    const length = this.product.listProductStoreDetailDto.length;
    const prevIndex = currentIndex === 0 ? length - 1 : currentIndex - 1;
    this.selectedDetail = this.product.listProductStoreDetailDto[prevIndex];
    this.modalImageIndex = prevIndex;
    this.currentImageIndex = prevIndex;
  }

  onSearch(): void {
    console.log('Search:', this.searchQuery);
  }

  viewRelatedProduct(productId: string): void {
    this.router.navigate(['/ecommerce/product', productId]);
  }

  Math = Math;

  addToCart(): void {
    // Optionally: trigger API/add to cart store here
  }
  chatWithSeller(): void {
    console.log('Chat with seller');
  }

  selectDeliveryMethod(method: string): void {
    this.deliveryMethod = method;
    if (method === this.deliveryMethodHome) {

    } else {
      this.deliveryAddress.ward = '';
      this.deliveryAddress.province = '';
    }
  }
  
  getTotalReviews(): number {
    return 0;
  }
  getStock(){
    return this.selectedDetail?.count;
  }
  hasColor(){
    return this.product?.listProductStoreDetailDto 
    && this.product.listProductStoreDetailDto.some(detail => detail.colorCode)
  }

  hasVersion(){
    return this.product?.listProductStoreDetailDto 
    && this.product.listProductStoreDetailDto.some(detail => detail.capacityCode)
  }

  @HostListener('touchstart', ['$event'])
  onTouchStart(event: TouchEvent): void {
    this.touchStartX = event.touches[0].clientX;
  }

  @HostListener('touchend', ['$event'])
  onTouchEnd(event: TouchEvent): void {
    if (!this.touchStartX) return;
    const touchEndX = event.changedTouches[0].clientX;
    const diff = this.touchStartX - touchEndX;
    
    if (Math.abs(diff) > 50) {
      if (diff > 0) {
        this.nextImage();
      } else {
        this.prevImage();
      }
    }
    this.touchStartX = null;
  }

  private touchStartX: number | null = null;
}

