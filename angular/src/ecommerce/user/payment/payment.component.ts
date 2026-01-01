import { Component, Injector, ChangeDetectionStrategy, ViewChild } from '@angular/core';
import { AppComponentBase } from '../../../shared/app-component-base';
import { appModuleAnimation } from '../../../shared/animations/routerTransition';
import { BsModalRef } from '@node_modules/ngx-bootstrap/modal';
import { UploadFileComponent } from '@ecommerce/component/file-upload/file-upload.component';
import { TransactionRequestModel, TransactionServiceProxy } from '@shared/service-proxies/service-proxies';
@Component({
    selector: 'payment',
    templateUrl: './payment.component.html',
    styleUrls: ['./payment.component.scss'],
    animations: [appModuleAnimation()],
  })

  export class PaymentComponent extends AppComponentBase {

    transactionId: number;

    @ViewChild('fileUpload')
    fileUploadComp!: UploadFileComponent;

    constructor(
      injector: Injector,
      private bsModalRef: BsModalRef,
      private transaction: TransactionServiceProxy
    ) {
        super(injector);
    }

    confirmPayment(){
      const files: File[] = this.fileUploadComp.files;
      
      if(files.length==0){
        abp.notify.warn(this.l("PleaseUploadFile"));
      }

      const uploadPayload = files.map((file) => ({
          data: file,
          fileName: file.name,
      }));

      this.transaction.paymentWithEvidence(this.transactionId, uploadPayload)
        .subscribe({
          next: () => {
            abp.notify.success(this.l('PaymentSubmitSuccess'));

            this.onCancel();
          },
          error: (err) => {
            abp.notify.error(this.l('PaymentError'));
          }
        });
    }

    onCancel(){
        this.bsModalRef.hide();
    }
  }
