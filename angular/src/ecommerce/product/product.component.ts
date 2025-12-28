import { Component, OnInit, HostListener } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
  productId: string | null = null;
  currentImageIndex = 0;
  selectedColor = 'white';
  selectedModel = 'standard';
  quantity = 1;
  descriptionExpanded = true;
  showSpecs = false;
  showReviews = false;
  showQA = false;
  isFollowing = false;
  showImageModal = false;
  modalImageIndex = 0;
  searchQuery = '';
  variantViewMode: 'buttons' | 'dropdown' = 'buttons';
  promotionEndTime: Date = new Date(Date.now() + 24 * 60 * 60 * 1000); // 24 hours from now
  timeRemaining = { hours: 0, minutes: 0, seconds: 0 };

  // Product data
  product = {
    id: '1',
    name: 'Ấm Điện Đun Nước 2.5L 1500W Tự Động Tắt',
    images: [
      'https://via.placeholder.com/400x400/ff6b35/ffffff?text=Electric+Kettle+1',
      'https://via.placeholder.com/400x400/ff8c42/ffffff?text=Electric+Kettle+2',
      'https://via.placeholder.com/400x400/ffa07a/ffffff?text=Electric+Kettle+3',
      'https://via.placeholder.com/400x400/ffb347/ffffff?text=Electric+Kettle+4'
    ],
    rating: 4.5,
    reviewCount: 1250,
    sold: 8500,
    currentPrice: 299000,
    originalPrice: 450000,
    discount: 33,
    freeShipping: true,
    estimatedDelivery: '2-3 ngày',
    vouchers: [
      { text: 'Giảm 50K', type: 'discount' },
      { text: 'Freeship', type: 'shipping' }
    ],
    colors: [
      { name: 'Trắng', value: 'white', available: true },
      { name: 'Đen', value: 'black', available: true },
      { name: 'Xanh', value: 'blue', available: true },
      { name: 'Hồng', value: 'pink', available: false }
    ],
    models: [
      { name: 'Tiêu chuẩn', value: 'standard' },
      { name: 'Cao cấp', value: 'premium' },
      { name: 'Pro', value: 'pro' }
    ],
    specifications: {
      capacity: '2.5L',
      power: '1500W',
      material: 'Nhựa PP cao cấp',
      safety: 'Tự động tắt khi sôi, chống tràn, chống quá nhiệt',
      dimensions: '20 x 18 x 25 cm',
      weight: '0.8 kg',
      warranty: '12 tháng'
    },
    reviews: [
      {
        user: 'Nguyễn Văn A',
        rating: 5,
        date: '2024-01-15',
        comment: 'Sản phẩm rất tốt, đun nước nhanh, tự động tắt an toàn. Giao hàng nhanh.',
        helpful: 12
      },
      {
        user: 'Trần Thị B',
        rating: 4,
        date: '2024-01-10',
        comment: 'Ấm đẹp, giá hợp lý. Chỉ hơi ồn một chút khi đun.',
        helpful: 8
      },
      {
        user: 'Lê Văn C',
        rating: 5,
        date: '2024-01-05',
        comment: 'Rất hài lòng với sản phẩm. Đúng như mô tả.',
        helpful: 15
      }
    ],
    starDistribution: {
      5: 850,
      4: 250,
      3: 100,
      2: 30,
      1: 20
    },
    seller: {
      name: 'Shop Điện Gia Dụng',
      avatar: 'https://via.placeholder.com/60x60/ff6b35/ffffff?text=Shop',
      rating: 4.8,
      followers: 12500,
      responseRate: '98%',
      responseTime: '< 1 giờ',
      isOfficial: true,
      badge: 'Official Store'
    },
    highlights: [
      { icon: 'fa-bolt', text: 'Đun nước nhanh trong 3-5 phút' },
      { icon: 'fa-shield-alt', text: 'Tự động tắt khi sôi, an toàn tuyệt đối' },
      { icon: 'fa-truck', text: 'Miễn phí vận chuyển toàn quốc' },
      { icon: 'fa-award', text: 'Bảo hành chính hãng 12 tháng' },
      { icon: 'fa-leaf', text: 'Tiết kiệm điện năng' }
    ],
    qa: [
      {
        question: 'Ấm có tự động tắt khi sôi không?',
        answer: 'Có, ấm có tính năng tự động tắt khi nước sôi để đảm bảo an toàn và tiết kiệm điện.',
        date: '2024-01-10'
      },
      {
        question: 'Có thể đun được bao nhiêu lít nước?',
        answer: 'Ấm có dung tích 2.5L, phù hợp cho gia đình 2-4 người sử dụng.',
        date: '2024-01-08'
      },
      {
        question: 'Thời gian bảo hành là bao lâu?',
        answer: 'Sản phẩm được bảo hành chính hãng 12 tháng, đổi mới trong 7 ngày đầu nếu có lỗi.',
        date: '2024-01-05'
      }
    ],
    relatedProducts: [
      {
        id: '2',
        name: 'Ấm Điện 1.7L 1200W',
        image: 'https://via.placeholder.com/200x200/ff6b35/ffffff?text=Kettle+1',
        price: 199000,
        originalPrice: 299000,
        rating: 4.3,
        sold: 3200
      },
      {
        id: '3',
        name: 'Ấm Điện 3L 1800W',
        image: 'https://via.placeholder.com/200x200/ff8c42/ffffff?text=Kettle+2',
        price: 399000,
        originalPrice: 550000,
        rating: 4.7,
        sold: 5600
      },
      {
        id: '4',
        name: 'Ấm Điện Inox 2L',
        image: 'https://via.placeholder.com/200x200/ffa07a/ffffff?text=Kettle+3',
        price: 349000,
        originalPrice: 480000,
        rating: 4.5,
        sold: 2100
      },
      {
        id: '5',
        name: 'Ấm Điện Thủy Tinh 1.5L',
        image: 'https://via.placeholder.com/200x200/ffb347/ffffff?text=Kettle+4',
        price: 249000,
        originalPrice: 350000,
        rating: 4.2,
        sold: 1800
      }
    ]
  };

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.productId = this.route.snapshot.paramMap.get('id');
    // In a real app, you would fetch product data based on ID
    this.startCountdown();
    this.updateVariantPrice();
  }

  startCountdown(): void {
    setInterval(() => {
      const now = new Date().getTime();
      const distance = this.promotionEndTime.getTime() - now;

      if (distance > 0) {
        this.timeRemaining.hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        this.timeRemaining.minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        this.timeRemaining.seconds = Math.floor((distance % (1000 * 60)) / 1000);
      } else {
        this.timeRemaining = { hours: 0, minutes: 0, seconds: 0 };
      }
    }, 1000);
  }

  goBack(): void {
    this.location.back();
  }

  shareProduct(): void {
    // Share functionality
    if (navigator.share) {
      navigator.share({
        title: this.product.name,
        text: 'Xem sản phẩm này trên E-Shop',
        url: window.location.href
      });
    }
  }

  goToCart(): void {
    this.router.navigate(['/ecommerce/cart']);
  }

  nextImage(): void {
    this.currentImageIndex = (this.currentImageIndex + 1) % this.product.images.length;
  }

  prevImage(): void {
    this.currentImageIndex = this.currentImageIndex === 0 
      ? this.product.images.length - 1 
      : this.currentImageIndex - 1;
  }

  goToImage(index: number): void {
    this.currentImageIndex = index;
  }

  selectColor(color: string): void {
    this.selectedColor = color;
    this.updateVariantPrice();
  }

  selectModel(model: string): void {
    this.selectedModel = model;
    this.updateVariantPrice();
  }

  updateVariantPrice(): void {
    // In a real app, fetch price based on selected variant
    // For demo, we'll adjust price slightly based on model
    const basePrice = 299000;
    const priceMultipliers: { [key: string]: number } = {
      'standard': 1,
      'premium': 1.2,
      'pro': 1.5
    };
    this.product.currentPrice = Math.round(basePrice * (priceMultipliers[this.selectedModel] || 1));
  }

  increaseQuantity(): void {
    this.quantity++;
  }

  decreaseQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  toggleDescription(): void {
    this.descriptionExpanded = !this.descriptionExpanded;
  }

  toggleSpecs(): void {
    this.showSpecs = !this.showSpecs;
  }

  toggleReviews(): void {
    this.showReviews = !this.showReviews;
  }

  toggleQA(): void {
    this.showQA = !this.showQA;
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
    this.modalImageIndex = (this.modalImageIndex + 1) % this.product.images.length;
    this.currentImageIndex = this.modalImageIndex;
  }

  prevModalImage(): void {
    this.modalImageIndex = this.modalImageIndex === 0 
      ? this.product.images.length - 1 
      : this.modalImageIndex - 1;
    this.currentImageIndex = this.modalImageIndex;
  }

  toggleVariantViewMode(): void {
    this.variantViewMode = this.variantViewMode === 'buttons' ? 'dropdown' : 'buttons';
  }

  onSearch(): void {
    // Search functionality
    console.log('Search:', this.searchQuery);
  }

  viewRelatedProduct(productId: string): void {
    this.router.navigate(['/ecommerce/product', productId]);
  }

  // Expose Math to template
  Math = Math;

  toggleFollow(): void {
    this.isFollowing = !this.isFollowing;
  }

  addToCart(): void {
    // Add to cart logic
    console.log('Added to cart:', {
      productId: this.productId,
      color: this.selectedColor,
      model: this.selectedModel,
      quantity: this.quantity
    });
    // Show success message or navigate to cart
  }

  buyNow(): void {
    // Buy now logic
    this.addToCart();
    this.router.navigate(['/ecommerce/cart']);
  }

  chatWithSeller(): void {
    // Chat functionality
    console.log('Chat with seller');
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
    return Object.values(this.product.starDistribution).reduce((a, b) => a + b, 0);
  }

  getColorName(value: string): string {
    const color = this.product.colors.find(c => c.value === value);
    return color ? color.name : value;
  }

  getColorHex(value: string): string {
    const colorMap: { [key: string]: string } = {
      'white': '#ffffff',
      'black': '#000000',
      'blue': '#007bff',
      'pink': '#ff69b4'
    };
    return colorMap[value] || '#cccccc';
  }

  getModelName(value: string): string {
    const model = this.product.models.find(m => m.value === value);
    return model ? model.name : value;
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

