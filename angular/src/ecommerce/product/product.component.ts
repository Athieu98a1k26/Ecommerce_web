import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';
import { ProductStorePublicServiceProxy,
  ProductStoreDto,
  ProductStoreDetailDto,
  BaseRequest,
  ProductStoreDtoPagedResultDto, } from '@shared/service-proxies/service-proxies';
import { BsModalRef, BsModalService } from '@node_modules/ngx-bootstrap/modal';
import { OrderComponent } from '../order/order.component';
import { environment } from 'environments/environment';


@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  productId: string | null = null;
  currentImageIndex = 0;
  selectedDetail:ProductStoreDetailDto;
  countDetail: Number | any = 0;
  quantity = 1;

  showImageModal = false;
  modalImageIndex = 0;
  searchQuery = '';
  product: ProductStoreDto = null;
  isLoading = false;
  tabActive :string = 'specs';

  products: ProductStoreDto[] = [];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private productApi: ProductStorePublicServiceProxy,
    private _modalService: BsModalService,
    private productStorePublicService: ProductStorePublicServiceProxy
  ) {}

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id');
    if (this.productId) {
      this.fetchProductDetail(this.productId);
    }
    this.getFeaturedProducts()
  }

  getFeaturedProducts(): void {
    const input = new BaseRequest();
    // Set paging as you need (for example, maxResultCount, skipCount)
    input.storeCode =environment.storeCode;
    input.maxResultCount = 12;
    input.skipCount = 0;

    this.productStorePublicService.getPagingFeaturedProduct(input)
      .subscribe({
        next: (result: ProductStoreDtoPagedResultDto) => {
          this.products = result.items || [];
        },
        error: () => {
          this.products = [];
        }
      });
  }
  setTab(tab:string){
    this.tabActive = tab;
  }
  fetchProductDetail(id: string): void {
    this.isLoading = true;
    const parsedId = parseInt(id);
    this.productApi.getDetail(parsedId).subscribe({
      next: (result: any) => {
        this.product = result;
        // set default variant selections if available
        if (this.product && this.product?.listProductStoreDetailDto) {
          this.selectedDetail = this.product?.listProductStoreDetailDto.find(item => item.isActive);
        }
        this.countDetail = this.product?.listProductStoreDetailDto.length;
        this.isLoading = false;
      },
      error: _ => { this.isLoading = false; }
    });
  }

  goBack(): void {
    this.location.back();
  }

  shareProduct(): void {
    if (!this.product) return;
    if (navigator.share) {
      navigator.share({
        title: this.product.productName,
        text: 'Xem sản phẩm này trên E-Shop',
        url: window.location.href
      });
    }
  }

  goToCart(): void {
    this.router.navigate(['/ecommerce/cart']);
  }

  get activeCapacityCodeDetails() {
    return this.product?.listProductStoreDetailDto.filter(d => d.capacityCode);
  }

  get activeColorCodeDetails() {
    return this.product?.listProductStoreDetailDto.filter(d => d.colorCode);
  }

  nextImage(): void {
    if (!this.product?.listProductStoreDetailDto?.length) return;
    this.currentImageIndex = (this.currentImageIndex + 1) % this.countDetail;
  }

  prevImage(): void {
    if (!this.product?.listProductStoreDetailDto?.length) return;
    this.currentImageIndex = this.currentImageIndex === 0 
      ? this.countDetail - 1 
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
    const currentIndex = this.product?.listProductStoreDetailDto.findIndex(
      d => d === currentDetail
    );
    const length = this.countDetail;
    const nextIndex = (currentIndex + 1) % length;
    this.selectedDetail = this.product?.listProductStoreDetailDto[nextIndex];
    this.modalImageIndex = nextIndex;
    this.currentImageIndex = nextIndex;
  }

  prevModalImage(): void {
    if (!this.product?.listProductStoreDetailDto?.length) return;
    const currentDetail = this.selectedDetail;
    const currentIndex = this.product.listProductStoreDetailDto.findIndex(
      d => d === currentDetail
    );
    const length = this.countDetail;
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

  buyNow(): void {
    //this.addToCart();
    //this.router.navigate(['/ecommerce/cart']);
    let openOrder: BsModalRef;
    openOrder = this._modalService.show(
      OrderComponent,
      {
        class: 'modal-xl',
        initialState: {
          productStoreId: this.selectedDetail.id,
          quantity:this.quantity,
          product:this.product,
          selectedDetail:this.selectedDetail
        },
      }
    );
  }

  getStarArray(rating: number): number[] {
    const fullStars = Math.floor(rating);
    const hasHalfStar = rating % 1 >= 0.5;
    return Array(5).fill(0).map((_, i) => {
      if (i < fullStars) return 1;
      if (i === fullStars && hasHalfStar) return 0.5;
      return 0;
    });
  }

  getTotalReviews(): number {
    return 0;
  }
  getStock(){
    return this.selectedDetail?.count;
  }
  hasColor(){
    return this.product?.listProductStoreDetailDto 
    && this.product?.listProductStoreDetailDto.some(detail => detail.colorCode)
  }

  hasVersion(){
    return this.product?.listProductStoreDetailDto 
    && this.product?.listProductStoreDetailDto.some(detail => detail.capacityCode)
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

