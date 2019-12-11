import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-spinner',
  templateUrl: './spinner.component.html',
  styleUrls: ['./spinner.component.css']
})
export class SpinnerComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    /** spinner starts on init */
    //this.spinner.show();

    //setTimeout(() => {
    //  /** spinner ends after 5 seconds */
    //  this.spinner.hide();
    //}, 5000);
  }

}
