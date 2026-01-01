import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CartComponent } from './cart/cart.component';
import { AuthComponent } from './auth/auth.component';
import { ProductComponent } from './product/product.component';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';
import { EcommerceComponent } from './ecommerce.component';
import { ProductSearchComponent } from './product-search/product-search.component';
import { UserComponent } from './user/user.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: 'auth',
                component: AuthComponent
              },
            {
                path: '',
                component: EcommerceComponent,
                children: [
                    { path: '', redirectTo: 'auth', pathMatch: 'full' },
                    { path: 'home', component: HomeComponent },
                    { path: 'cart', component: CartComponent },
                    { path: 'product/:id', component: ProductComponent },
                    { path: 'product-search', component: ProductSearchComponent },
                    {
                        path: 'user',
                        component: UserComponent,
                        canActivate: [AppRouteGuard],
                      }
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class EcommerceRoutingModule { }
