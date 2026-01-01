export class OrderActionHelper {

  canOrder(action: string,orderStatus: string): boolean {
    if (!action) return false;

    switch (action) {
      case OrderAction.Confirmed:
        // nếu mới khởi tạo thì dc xác nhận
        if(orderStatus == OrderStatus.Init){
            return true;
        }
        return false;
      case OrderAction.Cancelled:
        // nếu mới khởi tạo thì dc hủy
        if(orderStatus == OrderStatus.Init){
            return true;
        }
        return false;
      default:
        return false;
    }
  }
}

export enum OrderAction {
    Confirmed = 'Confirmed',
    Cancelled = 'Cancelled',
} 

export enum OrderStatus {
    Init = 'Init',
    Confirmed = 'Confirmed',
    Cancelled = 'Cancelled',
} 