import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CartComponent } from './cart/cart.component';
import { AuthComponent } from './auth/auth.component';
import { AppRouteGuard } from '../shared/auth/auth-route-guard';
import { EcommerceComponent } from './app.component';

@NgModule({
    imports: [
        RouterModule.forChild([
            {
                path: '',
                component: EcommerceComponent,
                children: [
                    { path: '', redirectTo: 'auth', pathMatch: 'full' },
                    { path: 'home', component: HomeComponent },
                    { path: 'cart', component: CartComponent },
                    { path: 'auth', component: AuthComponent },
                ]
            }
        ])
    ],
    exports: [RouterModule]
})
export class EcommerceRoutingModule { }
