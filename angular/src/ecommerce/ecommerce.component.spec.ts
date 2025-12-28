import { TestBed, async } from "@angular/core/testing";
import { LayoutStoreService } from "../shared/layout/layout-store.service";
import { AppSessionService } from "../shared/session/app-session.service";
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { HttpClientJsonpModule } from "@angular/common/http";
import { HttpClientModule } from "@angular/common/http";
import { ModalModule } from "ngx-bootstrap/modal";
import { BsDropdownModule } from "ngx-bootstrap/dropdown";
import { CollapseModule } from "ngx-bootstrap/collapse";
import { TabsModule } from "ngx-bootstrap/tabs";
import { NgxPaginationModule } from "ngx-pagination";
import { RouterTestingModule } from "@angular/router/testing";
import { ServiceProxyModule } from "../shared/service-proxies/service-proxy.module";
import { SharedModule } from "../shared/shared.module";
import { HomeComponent } from "../app/home/home.component";
import { AboutComponent } from "../app/about/about.component";
import { AppComponent } from "@app/app.component";


describe("AppComponent", () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [
        HomeComponent,
        AboutComponent,

      ],
      imports: [
        BrowserAnimationsModule,
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        HttpClientModule,
        HttpClientJsonpModule,
        ModalModule.forChild(),
        BsDropdownModule.forRoot(),
        CollapseModule.forRoot(),
        TabsModule.forRoot(),
        RouterTestingModule,
        ServiceProxyModule,
        SharedModule.forRoot(),
        NgxPaginationModule,
      ],
      providers: [
        LayoutStoreService,
        {
          provide: AppSessionService,
          useValue: {
            application: {
              version: "",
              releaseDate: {
                format: function () {
                  return "";
                },
              },
            },
            getShownLoginName: function(){
              return 'admin';
            }
          },
        },
      ],
    });
    TestBed.compileComponents();
  });

  it("should create the app", async(() => {
    const fixture = TestBed.createComponent(AppComponent);
    const app = fixture.debugElement.componentInstance;
    expect(app).toBeTruthy();
  }));
  
});
