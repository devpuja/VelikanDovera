import { Component } from '@angular/core';
import { AuthGuard } from '../auth.guard';
import { AdminAuthGuard } from '../admin.auth.guard';

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html',
  //providers: [AuthGuard, AdminAuthGuard]
})
export class CounterComponent {
  public currentCount = 0;

  public incrementCounter() {
    this.currentCount++;
  }
}
