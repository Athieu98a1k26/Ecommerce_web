import { Component, OnInit, Injector, HostListener } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { AppSessionService } from '@shared/session/app-session.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent extends AppComponentBase implements OnInit {
  activeTab: 'login' | 'register' = 'login';
  loginForm: FormGroup;
  registerForm: FormGroup;
  submitting = false;
  showPassword = false;
  showConfirmPassword = false;
  passwordStrength = 0;
  showUserDropdown = false;
  isLoggedIn = false;
  userName = '';
  userAvatar = 'assets/img/user.png';

  constructor(
    injector: Injector,
    private fb: FormBuilder,
    private router: Router,
    public authService: AppAuthService,
    private sessionService: AppSessionService
  ) {
    super(injector);
  }

  ngOnInit(): void {
    this.initLoginForm();
    this.initRegisterForm();
    this.checkLoginStatus();
  }

  checkLoginStatus(): void {
    const user = this.sessionService.user;
    this.isLoggedIn = !!user;
    if (user) {
      this.userName = user.userName || user.name || 'Người dùng';
    }
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.user-menu')) {
      this.showUserDropdown = false;
    }
  }

  initLoginForm(): void {
    this.loginForm = this.fb.group({
      userNameOrEmailAddress: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false]
    });
  }

  initRegisterForm(): void {
    this.registerForm = this.fb.group({
      phoneNumber: ['', [Validators.required, this.phoneValidator]],
      email: ['', [Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
      agreeToTerms: [false, [Validators.requiredTrue]]
    }, { validators: this.passwordMatchValidator });

    // Watch password changes for strength indicator
    this.registerForm.get('password')?.valueChanges.subscribe(value => {
      this.calculatePasswordStrength(value);
    });
  }

  phoneValidator(control: AbstractControl): ValidationErrors | null {
    const phone = control.value;
    if (!phone) return null;
    
    // Vietnamese phone number pattern
    const phonePattern = /^(0|\+84)[3|5|7|8|9][0-9]{8}$/;
    return phonePattern.test(phone.replace(/\s/g, '')) ? null : { invalidPhone: true };
  }

  passwordMatchValidator(form: AbstractControl): ValidationErrors | null {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    
    if (!password || !confirmPassword) return null;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  calculatePasswordStrength(password: string): void {
    if (!password) {
      this.passwordStrength = 0;
      return;
    }

    let strength = 0;
    if (password.length >= 6) strength++;
    if (password.length >= 8) strength++;
    if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^a-zA-Z0-9]/.test(password)) strength++;

    this.passwordStrength = Math.min(strength, 5);
  }

  getPasswordStrengthClass(): string {
    if (this.passwordStrength <= 2) return 'weak';
    if (this.passwordStrength <= 3) return 'medium';
    return 'strong';
  }

  switchTab(tab: 'login' | 'register'): void {
    this.activeTab = tab;
    this.loginForm.reset();
    this.registerForm.reset();
    this.showPassword = false;
    this.showConfirmPassword = false;
    this.passwordStrength = 0;
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onLogin(): void {
    if (this.loginForm.invalid) {
      this.markFormGroupTouched(this.loginForm);
      return;
    }

    this.submitting = true;
    this.authService.authenticateModel.userNameOrEmailAddress = 
      this.loginForm.value.userNameOrEmailAddress;
    this.authService.authenticateModel.password = this.loginForm.value.password;
    this.authService.rememberMe = this.loginForm.value.rememberMe;

    this.authService.authenticate(() => {
      this.submitting = false;
    });
  }

  onRegister(): void {
    if (this.registerForm.invalid) {
      this.markFormGroupTouched(this.registerForm);
      return;
    }

    this.submitting = true;
    // TODO: Implement registration logic
    // For now, just show a message
    this.message.success('Đăng ký thành công!', 'Thành công');
    setTimeout(() => {
      this.submitting = false;
      this.switchTab('login');
    }, 1500);
  }

  onSocialLogin(provider: 'facebook' | 'google' | 'apple'): void {
    // TODO: Implement social login
    this.message.info(`Đăng nhập bằng ${provider} đang được phát triển`, 'Thông báo');
  }

  onOTPLogin(): void {
    // TODO: Implement OTP login
    this.message.info('Đăng nhập bằng SMS đang được phát triển', 'Thông báo');
  }

  onForgotPassword(): void {
    // TODO: Implement forgot password
    this.message.info('Chức năng quên mật khẩu đang được phát triển', 'Thông báo');
  }

  markFormGroupTouched(formGroup: FormGroup): void {
    Object.keys(formGroup.controls).forEach(key => {
      const control = formGroup.get(key);
      control?.markAsTouched();
      if (control instanceof FormGroup) {
        this.markFormGroupTouched(control);
      }
    });
  }

  getErrorMessage(controlName: string, form: FormGroup): string {
    const control = form.get(controlName);
    if (!control || !control.errors || !control.touched) return '';

    if (control.errors['required']) {
      return 'Trường này là bắt buộc';
    }
    if (control.errors['minlength']) {
      return `Tối thiểu ${control.errors['minlength'].requiredLength} ký tự`;
    }
    if (control.errors['email']) {
      return 'Email không hợp lệ';
    }
    if (control.errors['invalidPhone']) {
      return 'Số điện thoại không hợp lệ';
    }
    if (control.errors['passwordMismatch']) {
      return 'Mật khẩu không khớp';
    }
    return '';
  }

  toggleUserDropdown(): void {
    this.showUserDropdown = !this.showUserDropdown;
  }

  navigateToHome(): void {
    this.router.navigate(['/ecommerce/home']);
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

  scrollToAuth(): void {
    const authElement = document.querySelector('.auth-column');
    if (authElement) {
      authElement.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
  }
}

