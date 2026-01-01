export class TransactionActionHelper {
    canTransaction(action: string,transactionStatus: string): boolean {
      if (!action) return false;
  
      switch (action) {
        case TransactionAction.Payment:
          // nếu mới khởi tạo thì dc xác nhận
          if(transactionStatus == TransactionStatus.Init 
            || transactionStatus == TransactionStatus.Reject){
              return true;
          }
          return false;
        default:
          return false;
      }
    }
  }
  
  export enum TransactionAction {
      Payment = 'Payment', // thanh toán
  } 
  
  export enum TransactionStatus {
      Init = 'Init',
      AwaitingApproval = 'AwaitingApproval',
      Paid = 'Paid',
      Reject = 'Reject',
  } 