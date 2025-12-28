import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientJsonpModule } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxPaginationModule } from 'ngx-pagination';
import { EcommerceRoutingModule } from './ecommerce-routing.module';
import { HomeComponent } from './home/home.component';
import { CartComponent } from './cart/cart.component';
import { AuthComponent } from './auth/auth.component';
import { ProductComponent } from './product/product.component';
import { ServiceProxyModule } from '../shared/service-proxies/service-proxy.module';
import { SharedModule } from '../shared/shared.module';
import { EcommerceComponent } from './ecommerce.component';
import { HeaderComponent } from '@ecommerce/layout/header/header.component';
import { FooterComponent } from '@ecommerce/layout/footer/footer.component';
import {ProductItemComponent} from '@ecommerce/component/product-Item/product-item.component'
@NgModule({
    declarations: [
        EcommerceComponent,
        HomeComponent,
        CartComponent,
        AuthComponent,
        ProductComponent,
        HeaderComponent,
        FooterComponent,
        ProductItemComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        ModalModule.forChild(),
        BsDropdownModule,
        CollapseModule,
        TabsModule,
        EcommerceRoutingModule, 
        ServiceProxyModule,
        SharedModule,
        NgxPaginationModule,
    ],
    providers: []
})
export class EcommerceModule {}
