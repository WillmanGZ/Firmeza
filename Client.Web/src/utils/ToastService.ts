import Swal from 'sweetalert2';

export default class ToastService {
  static toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    timer: 3000,
    timerProgressBar: true,
    didOpen: (toast) => {
      toast.onmouseenter = Swal.stopTimer;
      toast.onmouseleave = Swal.resumeTimer;
    },
  });

  static success(message: string) {
    this.toast.fire({
      icon: 'success',
      text: `${message}`,
    });
  }

  static info(message: string) {
    this.toast.fire({
      icon: 'info',
      text: `${message}`,
    });
  }

  static warning(message: string) {
    this.toast.fire({
      icon: 'warning',
      text: `${message}`,
    });
  }

  static error(message: string) {
    this.toast.fire({
      icon: 'error',
      text: `${message}`,
    });
  }
}
